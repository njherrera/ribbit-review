using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GUI.Services;
using GUI.ViewModels;
using GUI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using NamedPipeAPI;
using CommunityToolkit.Mvvm.DependencyInjection;
using CSharpParser.Filters.Settings;

namespace GUI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var services = new ServiceCollection();
                FilesService filesService = new();
                services.AddSingleton<IFilesService>(filesService);
                Services = services.BuildServiceProvider();
                Ioc.Default.ConfigureServices(Services);

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(),
                };
                
                PipeManager.createRequestPipe();
            }

            base.OnFrameworkInitializationCompleted();
        }

        public new static App? Current => Application.Current as App;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider? Services { get; private set; }
    }
}