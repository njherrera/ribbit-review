using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GUI.Services;
using GUI.ViewModels;
using GUI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jering.Javascript.NodeJS;

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
            FilesService filesService = new();

            var services = new ServiceCollection();
            services.AddSingleton<IFilesService>(filesService);

            Services = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(Services);

           /* StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.NodeAndV8Options = "--inspect-brk");
            StaticNodeJSService.Configure<OutOfProcessNodeJSServiceOptions>(options => options.InvocationTimeoutMS = -1);*/

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(),
                };
                
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