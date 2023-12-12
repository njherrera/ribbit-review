using Avalonia;
using System;
using NamedPipeAPI;
using CSharpParser;
using CSharpParser.JSON_Objects;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CSharpMain;

class Program
{
    //// Initialization code. Don't use any Avalonia, third-party APIs or any
    //// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    //// yet and stuff might break.
    //[STAThread]
    //public static void Main(string[] args) => BuildAvaloniaApp()
    //    .StartWithClassicDesktopLifetime(args);

    //// Avalonia configuration, don't remove; also used by visual designer.
    //public static AppBuilder BuildAvaloniaApp()
    //    => AppBuilder.Configure<App>()
    //        .UsePlatformDetect()
    //        .WithInterFont()
    //        .LogToTrace();
    static void Main(string[] args)
    {
        string testJson = PipeManager.connectJsonPipe();
    }
}
