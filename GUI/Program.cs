using Avalonia;
using Serilog;
using System;

namespace GUI
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/RR.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont();
        }
    }
}