using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IoCFileOps.Services;
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
using CSharpParser;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;
using System.Text;
using System.Diagnostics;

namespace GUI.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private string edgeguardJson;
        public MainViewModel() 
        { 
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            edgeguardJson = "";
        }

        public ICommand ApplyFilterCommand { get; }

        private async void ApplyFilter()
        {
            /* select replay file(s) to apply filter to
             * call NamedPipeAPI.PipeManager to open up a request pipe for JSON file(s)
             * (PipeManager.js is always listening for request from NamedPipeAPI)
             * use JsonConvert.DeserializeObject to convert returned JSON file into GameConversions object (handle this in PipeManager?)
             * call Filter.addToQueue w/ GameConversions and PlaybackQueue objects as params
             * save resulting PlaybackQueue object as a JSON file
             * pass location of JSON file to playback dolphin
             */
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            var result = await SelectFile(cancelToken);

            PipeManager.sendRequest(result);
            string returnJson = PipeManager.connectJsonPipe();

            GameConversions fileConversions = GameConversions.jsonToConversions(returnJson);
            PlaybackQueue returnQueue = new PlaybackQueue();
            
            Edgeguards.addToQueue(fileConversions, returnQueue);
            edgeguardJson = JsonConvert.SerializeObject(returnQueue, Formatting.Indented);

            await SaveFile();
        }

        private async Task<string> SelectFile(CancellationToken token)
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

                var file = await filesService.OpenFileAsync();
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

                var stream = new MemoryStream(Encoding.Default.GetBytes((string)edgeguardJson));
                await using var writeStream = await file.OpenWriteAsync();
                await stream.CopyToAsync(writeStream);
            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
        }
    }
}
