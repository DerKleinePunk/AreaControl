using AreaControl.ViewModels;
using AreaControl.ViewModels.SplitViewPane;
using AreaControl.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;

namespace AreaControl;

public partial class App : Application, IEnableLogger
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        this.Log().Info("OnFrameworkInitializationCompleted ->");

        //Locator.CurrentMutable.UseLog4NetWithWrappingFullLogger(); --Init Logger in app
        Locator.CurrentMutable.RegisterLazySingleton<HomePageViewModel>(() => new HomePageViewModel());
        Locator.CurrentMutable.RegisterLazySingleton<SettingsViewModel>(() => new SettingsViewModel());

        Locator.CurrentMutable.Register(() => new HomePageView(), typeof(HomePageView));
        Locator.CurrentMutable.Register(() => new SettingsView(), typeof(SettingsView));

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        

        base.OnFrameworkInitializationCompleted();
    }
}
