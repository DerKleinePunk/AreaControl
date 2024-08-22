using System;
using System.Linq;
using System.Threading;

using Avalonia;
using Avalonia.ReactiveUI;
using log4net;
using log4net.Config;
using Splat;
using Splat.Log4Net;

namespace AreaControl.Desktop;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args)
    {
        GlobalContext.Properties["LogFile"] = "AreaControl";
        XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
        Locator.CurrentMutable.UseLog4NetWithWrappingFullLogger();

        var builder = BuildAvaloniaApp();
        return args.Contains("--drm") ?
            //SilenceConsole();
            // If Card0, Card1 and Card2 all don't work. You can also try:                 
            // return builder.StartLinuxFbDev(args);
            // return builder.StartLinuxDrm(args, "/dev/dri/card1");
            builder.StartLinuxDrm(args, "/dev/dri/card1", 1D) : builder.StartWithClassicDesktopLifetime(args);
    }


    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();

    //captures the console input and hide it. Otherwise you will see the console cursor blinking on the screen.
    /*private static void SilenceConsole()
    {
        new Thread(() =>
            {
                Console.CursorVisible = false;
                while(true)
                    Console.ReadKey(true);
            })
            { IsBackground = true }.Start();
    }*/
}
