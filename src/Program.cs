using Avalonia;
using System;
using System.Threading.Tasks;
using AnimeNotepad.Services;

namespace AnimeNotepad;

class Program
{
    public static string? InitialFilePath { get; private set; }

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
            LogService.Error("UnhandledException", "Excepción no controlada", eventArgs.ExceptionObject as Exception);
        TaskScheduler.UnobservedTaskException += (_, eventArgs) =>
        {
            LogService.Error("UnobservedTaskException", "Excepción de tarea no observada", eventArgs.Exception);
            eventArgs.SetObserved();
        };

        InitialFilePath = args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : null;
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
}
