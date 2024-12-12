using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GUI.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CSharpParser.JSON_Objects;
using System.Text;
using System.Diagnostics;
using GUI.Settings;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using GUI.Models;
using System.Text.Json;
using Jering.Javascript.NodeJS;
using Serilog;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace GUI.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private UserPaths? userPaths;

        [ObservableProperty]
        private FilterViewModel _activeFilterVM;

        [ObservableProperty]
        private string _userCode;

        [ObservableProperty]
        private CharacterType _selectedUserChar;

        [ObservableProperty]
        private CharacterType _selectedOpponentChar;

        [ObservableProperty]
        private LegalStageType _selectedStage;

        [ObservableProperty]
        public string _filterResult;

        public CharacterType[] AvailableChars { get; } = Enum.GetValues<CharacterType>();

        public LegalStageType[] AvailableStages { get; } = Enum.GetValues<LegalStageType>();

        private int? userCharId, opponentCharId, stageId;

        partial void OnUserCodeChanged(string value)
        {
            ActiveFilterVM.UserId = value.ToUpper();
        }
        partial void OnSelectedUserCharChanged(CharacterType value)
        {
            // checking for value == 0 because if the user selects a character, THEN selects "any character", the int value will be 0 and we want to set it to null instead
            // this (and the next two values) will be passed to JS to filter replays with, so we want to make sure that these values are always valid character/stage codes as per slippi js
            // default enum values have a value of 0, and since "Any Character" is our default enum value it then has a value of 0
            // this causes problems for slippi js, since melee character ID's start with falcon, representing him w/ 0 and incrementing up by 1 from there (and in CharacterType his enum value is 1)
            // therefore, we're assigning it to userCharId as (int)value -1 to get around that IF the enum value is that of a melee character (and not 0 for "any character")
            // similar thing happening w/ stageId, where again "any stage" has an enum value of 0 and we want to represent that as null for slippi js's sake
            if ((int) value == 0) 
            {
                this.userCharId = null;
            } else { this.userCharId = ((int)value - 1); }
        }

        partial void OnSelectedOpponentCharChanged(CharacterType value)
        {
            if ((int)value == 0)
            {
                this.opponentCharId = null;
            }
            else { this.opponentCharId = ((int)value - 1); }
        }

        partial void OnSelectedStageChanged(LegalStageType value)
        {
            if ((int)value == 0)
            {
                this.stageId = null;
            }
            else { this.stageId = ((int)value); }
        }

        public List<FilterViewModel> AvailableFilterVMs { get; } = new List<FilterViewModel>()
        {
            new EdgeguardViewModel()
        };

        public MainViewModel()
        {
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            ViewJsonCommand = new RelayCommand(ViewJson);
            SelectSlippiCommand = new RelayCommand(SelectSlippi);
            SelectLocalCommand = new RelayCommand(SelectLocal);
            _activeFilterVM = AvailableFilterVMs[0];
            ActiveFilterVM.IsLocalReplay = false;
            userPaths = UserPaths.CheckForPaths();
        }


        public ICommand ApplyFilterCommand { get; }

        private async void ApplyFilter()
        {
            FilterResult = "";
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            List<string>? selectedPaths = await SelectSlpFiles(cancelToken);
            if (selectedPaths is null) { return; }

            object[] args = GetInterOpArgs(selectedPaths);
            List<GameConversions>? requestedConversions = await GetAllConversions(args);

            if (requestedConversions is null && selectedPaths.Count() > 0)
            {
                FilterResult = "No matching games found";
                return;
            } else if (requestedConversions is null)
            {
                return;
            }
            PlaybackQueue conversionQueue = ActiveFilterVM.applyFilter(requestedConversions);

            if (conversionQueue.queue.Count > 0)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string filterJson = JsonSerializer.Serialize(conversionQueue, options);

                await SaveJsonFile(filterJson);
            } else FilterResult = "No instances of situation found";
        }

        private object[] GetInterOpArgs(List<string> selectedPaths)
        {
            Dictionary<string, string?> gameSettingsDict = new Dictionary<string, string?>
            { // once user has clicked the apply filter button, their config is locked-in and we can grab it from the active filter VM
                {"userId", ActiveFilterVM.UserId},
                {"userChar", this.userCharId.ToString()},
                {"oppChar", this.opponentCharId.ToString()},
                {"stageId", this.stageId.ToString()},
                {"isLocal", ActiveFilterVM.IsLocalReplay.ToString()}
            };
            string constraints = "";
            foreach (KeyValuePair<string, string?> kvp in gameSettingsDict)
            {
                constraints += string.Format("{0}:{1} ", kvp.Key, kvp.Value);
            }
            object[] args = { constraints, string.Join(",", selectedPaths) };
            return args;
        }
        public ICommand ViewJsonCommand { get; }

        private async void ViewJson()
        {
            if (userPaths is null)
            {
                bool userGavePath = await PromptForPaths();
                if (userGavePath == false) { return; }
            }
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            var result = await SelectJsonFile(cancelToken);
            if (result is null) { return; }

            // making an array of dolphin exe params because ArgumentList naturally handles paths with spaces in them
            // passing --cout gets us access to dolphin's output feed, and in the current playback build skipping through a queue json in the replay viewer breaks the playbackqueue
            string[] dolphinParamList = { "-i", result, "-e", userPaths.MeleeIsoPath, "--cout", "--hide-seekbar" };

            try
            {
                RunCmdPrompt(dolphinParamList);
            } 
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred when calling runCmdPrompt from ViewJson");
            }
        }
        private async Task<bool> PromptForPaths()
        {
            bool response = false;
            try
            {
                var box = MessageBoxManager.GetMessageBoxStandard("No path to a Melee Iso detected", "Would you like to choose a valid SSBM Iso to use for viewing Playback Queues?", ButtonEnum.YesNo);

                var result = await box.ShowAsync();
                if (result == ButtonResult.Yes)
                {
                    response = true;
                    userPaths = await UserPaths.CreateUserPaths();
                }
                else response = false;
            } catch (Exception ex) 
            { 
                Log.Error(ex, "Exception occurred when calling PromptForPaths from ViewJson");
            }
            return response;
        }
        public ICommand SelectSlippiCommand { get; }

        private void SelectSlippi()
        {
            ActiveFilterVM.IsLocalReplay = false;
        }

        public ICommand SelectLocalCommand { get; }

        private void SelectLocal()
        {
            ActiveFilterVM.IsLocalReplay = true;
        }
        private void RunCmdPrompt(string[] dolphinParams)
        {
            using (var dolphin = new Process())
            {
                // using ArgumentList because it naturally handles file path params with spaces in them
                // this means we don't need to worry about passing specifically verbatim string literals for paths
                ProcessStartInfo startInfo = new ProcessStartInfo()
                { ArgumentList =
                    {
                        dolphinParams[0],
                        dolphinParams[1],
                        dolphinParams[2],
                        dolphinParams[3],
                        dolphinParams[4],
                        dolphinParams[5],
                    }
                };
                startInfo.FileName = userPaths.PlaybackExePath;
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                dolphin.StartInfo = startInfo;

                dolphin.EnableRaisingEvents = true;
                dolphin.OutputDataReceived += (object sender, DataReceivedEventArgs args) =>
                {
                    if ((args.Data != null) && args.Data.Contains("NO_GAME"))
                    {
                        dolphin.Kill(); // after reaching the end of a playback queue, dolphin emits [NO_GAME] output and we kill the process at that point
                    }
                    //else Debug.WriteLine("received output: ", args.Data);
                };
                dolphin.ErrorDataReceived += (object sender, DataReceivedEventArgs args) => Debug.Write("error received: " + args.Data);

                dolphin.Start();
                dolphin.BeginOutputReadLine();
                dolphin.BeginErrorReadLine();
                dolphin.WaitForExit();
            }
        }
        
        public async Task<List<GameConversions>?> GetAllConversions(object[] interOpArgs)
        {
            List<GameConversions>? result;
            try
            {
                result = await StaticNodeJSService.InvokeFromFileAsync<List<GameConversions>>("./JavaScript/interop.js", "getAllConversions", interOpArgs);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred when calling getAllConversions from ApplyFilter");
                result = null;
            }
            return result;
        }

        // helper method used for selecting .slp file(s) to apply filter to
        private async Task<List<string>?> SelectSlpFiles(CancellationToken token)
        {
            List<string> fullPaths = new List<string>();
            try
            {
                var filesService = Ioc.Default.GetService<IFilesService>();
                if (filesService is null)
                {
                    throw new NullReferenceException("Missing File Service instance.");
                }

                var files = await filesService.OpenSlpFilesAsync();
                var result = files?.ToList();

                if (result is null) { return null; } 
                foreach (Avalonia.Platform.Storage.IStorageFile file in result)
                {
                    string filePath = file.Path.ToString();
                    fullPaths.Add(filePath);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred when calling SelectSlpFiles from ApplyFilter");
                return fullPaths;
            }
            return fullPaths;
        }

        // helper method for selecting .json file to load into playback dolphin
        // this can only select *one* .json file
        private async Task<string?> SelectJsonFile(CancellationToken token)
        {
            string fullPath = "";
            try
            {
                var filesService = Ioc.Default.GetService<IFilesService>();
                if (filesService is null)
                {
                    throw new NullReferenceException("Missing File Service instance.");
                }

                var file = await filesService.OpenJsonFileAsync();
                if (file is null) return null;

                var result = file.Path.ToString();
                fullPath = result.Replace(@"file:///", "");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred when calling SelectJsonFile from ViewJson");
            }
            return fullPath;
        }
        private async Task SaveJsonFile(string json)
        {
            try
            {
                var filesService = Ioc.Default.GetService<IFilesService>();
                if (filesService is null) throw new NullReferenceException("Missing File Service instance.");

                var file = await filesService.SaveFileAsync();
                if (file is null) return;

                var stream = new MemoryStream(Encoding.Default.GetBytes((string)json));
                await using var writeStream = await file.OpenWriteAsync();
                await stream.CopyToAsync(writeStream);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred when calling SaveJsonFile from ApplyFilter");
            }
        }

    }
}
