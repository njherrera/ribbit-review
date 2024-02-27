using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using IoCFileOps.Services;

namespace GUI.Services
{
    public class FilesService : IFilesService
    {
        private readonly Window _target;

        public FilesService(Window target)
        {
            _target = target;
        }

        public async Task<IStorageFile?> OpenFileAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Choose Slippi replay file",
                FileTypeFilter = new[] { SlpFile }, // only shows .slp files in the choose file window
                AllowMultiple = false
            });

            return files.Count >= 1 ? files[0] : null;
        }

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



    }
}