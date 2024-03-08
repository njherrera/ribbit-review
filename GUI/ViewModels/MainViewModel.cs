﻿using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Xml;

namespace GUI.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        // TODO: (2) add filter settings/choose filter element to UI
        // TODO: (4) add game settings element to UI
        private UserPaths userPaths;

        [ObservableProperty]
        private GameSettings selectedSettings;

        public MainViewModel() 
        { 
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            ViewJsonCommand = new RelayCommand(ViewJson);
            selectedSettings = new GameSettings();
            checkForPaths();
        }

        
        public ICommand ApplyFilterCommand { get; }

        private async void ApplyFilter()
        {
            /* currently applies filter to ONE replay file
             * TODO: (3) implement application to multiple games
             */
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            var result = await SelectSlpFile(cancelToken);

            PipeManager.sendRequest(result);
            string returnJson = PipeManager.connectJsonPipe();

            GameConversions fileConversions = GameConversions.jsonToConversions(returnJson);
            PlaybackQueue returnQueue = new PlaybackQueue();

            Edgeguards.addToQueue(fileConversions, returnQueue);
            string filterJson = JsonConvert.SerializeObject(returnQueue, Newtonsoft.Json.Formatting.Indented);

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
                    else Debug.WriteLine("received output: ", args.Data);
                };
                dolphin.ErrorDataReceived += (object sender, DataReceivedEventArgs args) => Debug.Write("error received: " + args.Data);

                dolphin.Start();
                dolphin.BeginOutputReadLine();
                dolphin.BeginErrorReadLine();
                dolphin.WaitForExit();
            }
        }
        
        // helper method used for selecting .slp file(s) to apply filter to
        private async Task<string> SelectSlpFile(CancellationToken token)
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

                var file = await filesService.OpenSlpFileAsync();
                if (file is null) return string.Empty;

                var result = file.Path.ToString();
                fullPath = result;
            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
            return fullPath;
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
