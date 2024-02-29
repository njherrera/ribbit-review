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

namespace GUI.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private string filterJson;
        private UserPaths paths;

        [ObservableProperty]
        private GameSettings selectedSettings;

        public MainViewModel() 
        { 
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            ViewJsonCommand = new RelayCommand(ViewJson);
            filterJson = "";
            selectedSettings = new GameSettings();
        }

        
        public ICommand ApplyFilterCommand { get; }

        private async void ApplyFilter()
        {
            /* currently applies filter to ONE replay file
             */
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            var result = await SelectSlpFile(cancelToken);

            PipeManager.sendRequest(result);
            string returnJson = PipeManager.connectJsonPipe();

            GameConversions fileConversions = GameConversions.jsonToConversions(returnJson);
            PlaybackQueue returnQueue = new PlaybackQueue();
            
            Edgeguards.addToQueue(fileConversions, returnQueue);
            filterJson = JsonConvert.SerializeObject(returnQueue, Formatting.Indented);

            await SaveFile();
        }

        private async Task<string> SelectSlpFile(CancellationToken token)
        {
            string fullPath = "";
            ErrorMessages?.Clear();
            try
            {
                var filesService = App.Current?.Services?.GetService<IFilesService>();
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

        private async Task SaveFile()
        {
            ErrorMessages?.Clear();
            try
            {
                var filesService = App.Current?.Services?.GetService<IFilesService>();
                if (filesService is null) throw new NullReferenceException("Missing File Service instance.");

                var file = await filesService.SaveFileAsync();
                if (file is null) return;

                var stream = new MemoryStream(Encoding.Default.GetBytes((string)filterJson));
                await using var writeStream = await file.OpenWriteAsync();
                await stream.CopyToAsync(writeStream);
            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
        }

        public ICommand ViewJsonCommand { get; }

        private async void ViewJson()
        {
            checkForPaths();
        }
        private async void checkForPaths()
        {
#if DEBUG
            string devPath = @"Q:/programming/ribbit-review/GUI/UserVars.xml";
            if (File.Exists(devPath)){
                paths = deserializeUserPaths(devPath);
            } else {
                var AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string defaultPlaybackPath = Path.Combine(AppData, "Slippi Launcher", "playback", "Slippi Dolphin.exe");

                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;
                var result = await SelectMeleeIso(cancelToken);
                
                paths = new UserPaths(defaultPlaybackPath, result);
                serializeUserPaths(paths, devPath);
            }
#endif
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){
                var AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string pathsLocation = Path.Combine(AppData, "Ribbit Review", "UserPaths.xml");
                if (File.Exists(pathsLocation))
                {
                    paths = deserializeUserPaths(pathsLocation);
                } else {
                    string defaultPlaybackPath = Path.Combine(AppData, "Slippi Launcher", "playback", "Slippi Dolphin.exe");

                    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                    CancellationToken cancelToken = cancelTokenSource.Token;
                    var result = await SelectMeleeIso(cancelToken);

                    paths = new UserPaths(defaultPlaybackPath, result);
                    serializeUserPaths(paths, devPath);
                }
            }
            /* check for serialized xml instance of UserPaths.cs
            * if no saved instance:
            *      check default playback dolphin path for instance of dolphin .exe at
            *          windows: %APPDATA/Slippi Launcher/playback
            *          linux: ~/.config/Slippi Launcher/playback
            *          macOS(?): ~/Library/Application Support/Slippi Launcher/playback
            *          (paths for slippi dolphin installs are LOCKED)
            *      prompt for melee .iso path
            * if saved instance:
            *      read paths from saved UserPaths
            *      check that paths are valid (files exist)
            */
        }

        private void serializeUserPaths(UserPaths paths, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserPaths));
            // To write to a file, create a StreamWriter object.  
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
                var filesService = App.Current?.Services?.GetService<IFilesService>();
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
    }
}
