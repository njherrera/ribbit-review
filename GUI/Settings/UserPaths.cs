using CommunityToolkit.Mvvm.DependencyInjection;
using GUI.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GUI.Settings
{
    public class UserPaths
    {
        public string PlaybackExePath;

        public string MeleeIsoPath;

        public UserPaths(string ExePath, string IsoPath) 
        {
            PlaybackExePath = ExePath;
            MeleeIsoPath = IsoPath;
        }

        private UserPaths() { }

        public static UserPaths? CheckForPaths()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string pathsLocation = Path.Combine(AppData, "Ribbit Review", "UserPaths.xml");
                if (File.Exists(pathsLocation))
                {
                    return deserializeUserPaths(pathsLocation);
                }
                else
                {
                    return null;
                }
            } else return null;
        }

        private static void serializeUserPaths(UserPaths paths, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserPaths));
            StreamWriter myWriter = new StreamWriter(fileName);
            serializer.Serialize(myWriter, paths);
            myWriter.Close();
        }

        private static UserPaths deserializeUserPaths(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserPaths));
            using var myFileStream = new FileStream(filePath, FileMode.Open);
            UserPaths savedPaths = (UserPaths)serializer.Deserialize(myFileStream);
            return savedPaths;
        }

        private static async Task<string> SelectMeleeIso(CancellationToken token)
        {
            string fullPath = "";
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
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred when calling SelectMeleeIso after prompting for user paths");
            }
            return fullPath;
        }

        public static async Task<UserPaths> CreateUserPaths()
        {
            // dolphin install paths are locked, so this should be the same for every user
            // windows: %APPDATA%/Slippi Launcher/playback
            // linux: ~/.config/Slippi Launcher/playback
            // macOS: ~/Library/Application Support/Slippi Launcher/playback
            var AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string defaultPlaybackPath = Path.Combine(AppData, "Slippi Launcher", "playback", "Slippi Dolphin.exe");
            string RRFolder = Path.Combine(AppData, "Ribbit Review");
            string pathsLocation = Path.Combine(RRFolder, "UserPaths.xml");

            if (!Directory.Exists(RRFolder))
            {
                Directory.CreateDirectory(RRFolder);
            }

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            var result = await SelectMeleeIso(cancelToken);

            UserPaths userPaths = new UserPaths(defaultPlaybackPath, result);
            serializeUserPaths(userPaths, pathsLocation);
            return userPaths;
        }

    }
}
