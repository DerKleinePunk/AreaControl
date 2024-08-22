using AreaControl.ViewModels.SplitViewPane;
using AreaControl.ViewModels;
using AreaControl.Views;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System.Collections.Generic;
using System;
using Splat;
using ReactiveUI;

namespace AreaControl;

public class ViewLocator : IDataTemplate, IEnableLogger
{
    private readonly Dictionary<Type, Func<Control?>> _locator = new();

    public ViewLocator()
    {
        RegisterViewFactory<HomePageViewModel, HomePageView>();
        RegisterViewFactory<SettingsViewModel, SettingsView>();
        
    }

    public Control Build(object? data)
    {
        if (data is null)
        {
            this.Log().Error("No VM provided");
            return new TextBlock { Text = "No VM provided" };
        }

        _locator.TryGetValue(data.GetType(), out var factory);

        if (factory == null)
        {
            this.Log().Error($"VM Not Registered: {data.GetType()}");
        }

        return factory?.Invoke() ?? new TextBlock { Text = $"VM Not Registered: {data.GetType()}" };
    }

    public bool Match(object? data)
    {
        return data is ReactiveObject;
    }

    private void RegisterViewFactory<TViewModel, TView>()
        where TViewModel : class
        where TView : Control
        => _locator.Add(
            typeof(TViewModel),
            Design.IsDesignMode
                ? Activator.CreateInstance<TView>
                : () => Locator.Current.GetService<TView>());
}