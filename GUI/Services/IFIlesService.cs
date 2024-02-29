using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace GUI.Services;

public interface IFilesService
{
    public Task<IStorageFile?> OpenSlpFileAsync();
    public Task<IStorageFile?> OpenIsoFileAsync();
    public Task<IStorageFile?> OpenExeFileAsync();
    public Task<IStorageFile?> SaveFileAsync();
}