using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using GUI.Services;

namespace GUI.Services
{
    public class FilesService : IFilesService
    {
        private Window _target;

        public void setTargetWindow(Window target)
        {
            _target = target;
        }

        public async Task<IStorageFile?> OpenSlpFileAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Choose Slippi replay file",
                FileTypeFilter = new[] { SlpFile }, // only shows .slp files in the choose file window
                AllowMultiple = false
            });

            return files.Count >= 1 ? files[0] : null;
        }

        public async Task<IStorageFile?> OpenJsonFileAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Choose Slippi replay file",
                FileTypeFilter = new[] { JsonFile }, // only shows .json files in the choose file window
                AllowMultiple = false
            });

            return files.Count >= 1 ? files[0] : null;
        }

        public async Task<IStorageFile?> OpenIsoFileAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Choose your vanilla Melee v1.02 iso",
                FileTypeFilter = new[] { MeleeIso }, // only shows valid melee .iso file (assuming it contains "Melee" and "1.02" in that order, and ends with .iso)
                AllowMultiple = false
            });

            return files.Count >= 1 ? files[0] : null;
        }

        public async Task<IStorageFile?> OpenExeFileAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Choose Playback Dolphin .exe",
                FileTypeFilter = new[] { DolphinExe }, // only shows instances of Slippi Dolphin
                AllowMultiple = false
            });

            return files.Count >= 1 ? files[0] : null;
        }
        public async Task<IStorageFile?> SaveFileAsync()
        {
            return await _target.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "Save JSON file",
                DefaultExtension = ".json",
                ShowOverwritePrompt = true,
                FileTypeChoices = new[] { JsonFile }
            });
        }

        public static FilePickerFileType JsonFile { get; } = new("JSON file")
        {
            Patterns = new[] { "*.json" }
        };

        public static FilePickerFileType SlpFile { get; } = new("Slippi replay files")
        {
            Patterns = new[] { "*.slp" }
        };

        public static FilePickerFileType DolphinExe { get; } = new("Dolphin instance")
        {
            Patterns = new[] { "Slippi Dolphin.exe" }
        };

        public static FilePickerFileType MeleeIso { get; } = new("SSBM Iso")
        {
            Patterns = new[] {"*Melee*1.02*.iso"}
        };

    }
}