using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GUI.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using NamedPipeAPI;
using CSharpParser.JSON_Objects;
using CSharpParser.Filters;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;
using System.Text;
using System.Diagnostics;
using CSharpParser.SlpJSObjects;
using GUI.Settings;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using CommunityToolkit.Mvvm.DependencyInjection;
using CSharpParser.Filters.Settings;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using GUI.Models;
using GUI.Helpers.StringReaderStream;
using System.Collections.Immutable;

namespace GUI.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private UserPaths userPaths;

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

        public CharacterType[] AvailableChars { get; } = Enum.GetValues<CharacterType>();

        public LegalStageType[] AvailableStages { get; } = Enum.GetValues<LegalStageType>();

        new Dictionary<string, int?> gameSettingsDict = new Dictionary<string, int?>
        {
            {"uId", null },
            {"uChar", null},
            {"oChar", null},
            {"stage", null }
        };

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
            else { this.stageId = ((int)value - 1); }
        }

        public List<FilterViewModel> AvailableFilterVMs { get; } = new List<FilterViewModel>()
        {
            new EdgeguardViewModel()
        };

        public MainViewModel()
        {
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            ViewJsonCommand = new RelayCommand(ViewJson);
            _activeFilterVM = AvailableFilterVMs[0];
            checkForPaths();
        }


        public ICommand ApplyFilterCommand { get; }

        private async void ApplyFilter()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            var result = await SelectSlpFiles(cancelToken);

            Dictionary<string, string?> gameSettingsDict = new Dictionary<string, string?> // using this to pass GameSettings constraints to JS
            { // once user has clicked the apply filter button, their config is locked-in and we can grab it from the active filter VM
                {"userId", ActiveFilterVM.UserId},
                {"userChar", this.userCharId.ToString()},
                {"oppChar", this.opponentCharId.ToString()},
                {"stageId", this.stageId.ToString()}
            };
            string constraints = "";
            foreach (KeyValuePair<string, string?> kvp in gameSettingsDict)
            {
                constraints += string.Format("{0}:{1} ", kvp.Key, kvp.Value);
            }
            string requestedPaths = constraints.ToString() + "|" + string.Join(",", result);
            PipeManager.sendRequest(requestedPaths);

            List<GameConversions> requestedConversions = new List<GameConversions>();

            using (StringReaderStream jsonStream = new StringReaderStream(PipeManager.readJson()))
            using (StreamReader sr = new StreamReader(jsonStream))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                reader.SupportMultipleContent = true;
                var serializer = new JsonSerializer();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        GameConversions gc = serializer.Deserialize<GameConversions>(reader);
                        requestedConversions.Add(gc);
                    }
                }
            }

            PlaybackQueue returnQueue = new PlaybackQueue(); 
            ActiveFilterVM.applyFilter(requestedConversions, returnQueue);
            string filterJson = JsonConvert.SerializeObject(returnQueue, Newtonsoft.Json.Formatting.Indented);

            PipeManager.openRequestPipe();
            await SaveJsonFile(filterJson);
        }

        public ICommand ViewJsonCommand { get; }

        private async void ViewJson()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            var result = await SelectJsonFile(cancelToken);

            // making an array of dolphin exe params because ArgumentList naturally handles paths with spaces in them
            // passing --cout gets us access to dolphin's output feed, and in the current playback build skipping through a queue json in the replay viewer breaks the playbackqueue
            string[] dolphinParamList = { "-i", result, "-e", userPaths.MeleeIsoPath, "--cout", "--hide-seekbar" };
            runCmdPrompt(dolphinParamList);
        }


        private void runCmdPrompt(string[] dolphinParams)
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
        
        // helper method used for selecting .slp file(s) to apply filter to
        private async Task<List<string>> SelectSlpFiles(CancellationToken token)
        {
            List<string> fullPaths = new List<string>();
            ErrorMessages?.Clear();
            try
            {
                var filesService = Ioc.Default.GetService<IFilesService>();
                if (filesService is null)
                {
                    throw new NullReferenceException("Missing File Service instance.");
                }

                var files = await filesService.OpenSlpFilesAsync();
                var result = files.ToList();
                // .slp file names are in standard iso datetime format (YYYYMMDDTHHMMSS (date, T, time)), so we can use that to sort the list by date created without getting funky with avalonia IStorageFile properties
                // code still looks a bit stinky, but every .slp file is formatted the same way (the T inbetween date and time and no colons/hyphens/what have you between different parts make it a bit funky for parsing as a DateTime)
                result.Sort((x, y) =>
                {
                    ReadOnlySpan<char> xPathTrimmed = Path.GetFileNameWithoutExtension(x.Name.Substring(5));
                    DateTime xDT = new DateTime(int.Parse(xPathTrimmed.Slice(0, 4)), int.Parse(xPathTrimmed.Slice(4, 2)), int.Parse(xPathTrimmed.Slice(6, 2)), int.Parse(xPathTrimmed.Slice(9, 2)), int.Parse(xPathTrimmed.Slice(11, 2)), int.Parse(xPathTrimmed.Slice(13, 2)));
                    ReadOnlySpan<char> yPathTrimmed = Path.GetFileNameWithoutExtension(y.Name.Substring(5));
                    DateTime yDT = new DateTime(int.Parse(yPathTrimmed.Slice(0, 4)), int.Parse(yPathTrimmed.Slice(4, 2)), int.Parse(yPathTrimmed.Slice(6, 2)), int.Parse(yPathTrimmed.Slice(9, 2)), int.Parse(yPathTrimmed.Slice(11, 2)), int.Parse(yPathTrimmed.Slice(13, 2)));
                    return DateTime.Compare(xDT, yDT);
                });
                foreach (Avalonia.Platform.Storage.IStorageFile file in result)
                {
                    string filePath = file.Path.ToString();
                    fullPaths.Add(filePath);
                }
            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
            return fullPaths;
        }

        // helper method for selecting .json file to load into playback dolphin
        // this can only select *one* .json file
        private async Task<string> SelectJsonFile(CancellationToken token)
        {
            string fullPath = "";
            ErrorMessages?.Clear();
            try
            {
                var filesService = Ioc.Default.GetService<IFilesService>();
                if (filesService is null)
                {
                    throw new NullReferenceException("Missing File Service instance.");
                }

                var file = await filesService.OpenJsonFileAsync();
                if (file is null) return string.Empty;

                var result = file.Path.ToString();
                fullPath = result.Replace(@"file:///", "");
            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
            return fullPath;
        }
        private async Task SaveJsonFile(string json)
        {
            ErrorMessages?.Clear();
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
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
        }

        // checkForPaths() is run at the end of the MainViewModel constructor, and checks for a saved instance of the user's playback dolphin .exe and melee .iso paths
        // we need these paths in order to view a json file in playback dolphin, since they're both passed as arguments to command prompt
        // if the user has never booted up ribbit review before or their saved paths are missing, they'll be prompted for their melee .iso after the main view loads up and their paths will be saved to disk
        // if the user has booted up ribbit review, their paths will be loaded from disks and they can proceed to use the program
        public void checkForPaths()
        {
#if DEBUG
            string devPath = @"Q:/programming/ribbit-review/GUI/UserPaths.xml";
            if (File.Exists(devPath))
            {
                userPaths = deserializeUserPaths(devPath);
            } 
            else 
            { 
               CreateUserPaths(devPath); 
            }
#endif
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            { 
                var AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string pathsLocation = Path.Combine(AppData, "Ribbit Review", "UserPaths.xml");
                if (File.Exists(pathsLocation))
                {
                    userPaths = deserializeUserPaths(pathsLocation);
                } 
                else 
                {
                    string RRFolder = Path.Combine(AppData, "Ribbit Review");
                    if (!Directory.Exists(RRFolder))
                    {
                        Directory.CreateDirectory(RRFolder);
                    }
                    CreateUserPaths(pathsLocation); 
                }
            }
        }

        private void serializeUserPaths(UserPaths paths, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserPaths)); 
            StreamWriter myWriter = new StreamWriter(fileName);
            serializer.Serialize(myWriter, paths);
            myWriter.Close();
        }

        private UserPaths deserializeUserPaths(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserPaths));
            using var myFileStream = new FileStream(filePath, FileMode.Open);
            UserPaths savedPaths = (UserPaths)serializer.Deserialize(myFileStream);
            return savedPaths;
        }

        private async Task<string> SelectMeleeIso(CancellationToken token)
        {
            string fullPath = "";
            ErrorMessages?.Clear();
            try
            {
                var filesService = Ioc.Default.GetService<IFilesService>();
                if (filesService is null)
                {
                    throw new NullReferenceException("Missing File Service instance.");
                }

                var file = await filesService.OpenIsoFileAsync();
                if (file is null) return string.Empty;

                var result = file.Path.ToString();
                fullPath = result.Replace(@"file:///", ""); 
            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
            return fullPath;
        }

        private async void CreateUserPaths(string fileLocation)
        {
            // dolphin install paths are locked, so this should be the same for every user
            // windows: %APPDATA%/Slippi Launcher/playback
            // linux: ~/.config/Slippi Launcher/playback
            // macOS: ~/Library/Application Support/Slippi Launcher/playback
            var AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string defaultPlaybackPath = Path.Combine(AppData, "Slippi Launcher", "playback", "Slippi Dolphin.exe");

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            var result = await SelectMeleeIso(cancelToken);

            userPaths = new UserPaths(defaultPlaybackPath, result);
            serializeUserPaths(userPaths, fileLocation);
        }

    }
}
