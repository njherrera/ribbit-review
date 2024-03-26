using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace GUI.Services;

public interface IFilesService
{
    public Task<IReadOnlyList<IStorageFile>?> OpenSlpFilesAsync();
    public Task<IStorageFile?> OpenIsoFileAsync();
    public Task<IStorageFile?> OpenExeFileAsync();
    public Task<IStorageFile?> OpenJsonFileAsync();
    public Task<IStorageFile?> SaveFileAsync();
}