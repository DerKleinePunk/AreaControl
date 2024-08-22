using AreaControl.Models;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using AreaControl.ViewModels.SplitViewPane;
using System.Windows.Input;
using Splat;
using System.Collections.ObjectModel;
using System.Linq;

namespace AreaControl.ViewModels;

public class MainViewModel : ViewModelBase, IEnableLogger
{

    private readonly List<ListItemTemplate> _templates =
    [
        new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular", "Home"),
        new ListItemTemplate(typeof(SettingsViewModel), "CursorHoverRegular", "Settings"),
        /*new ListItemTemplate(typeof(TextPageViewModel), "TextNumberFormatRegular", "Text"),
        new ListItemTemplate(typeof(ValueSelectionPageViewModel), "CalendarCheckmarkRegular", "Value Selection"),
        new ListItemTemplate(typeof(ImagePageViewModel), "ImageRegular", "Images"),
        new ListItemTemplate(typeof(GridPageViewModel), "GridRegular", "Grids"),
        new ListItemTemplate(typeof(DragAndDropPageViewModel), "TapDoubleRegular", "Drang And Drop"),
        new ListItemTemplate(typeof(LoginPageViewModel), "LockRegular", "Login Form"),
        new ListItemTemplate(typeof(ChartsPageViewModel), "PollRegular", "Charts"),*/
    ];

    public MainViewModel()
    {
        TriggerPaneCommand = ReactiveCommand.Create(() =>
        {
            IsPaneOpen = !IsPaneOpen;
        });

        Items = new ObservableCollection<ListItemTemplate>(_templates);

        SelectedListItem = Items.First(vm => vm.ModelType == typeof(HomePageViewModel));
    }

    private bool _isPaneOpen;

    public bool IsPaneOpen
    {
        get => _isPaneOpen;
        set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
    }

    private ListItemTemplate? _selectedListItem;

    public ListItemTemplate? SelectedListItem
    {
        get => _selectedListItem;
        set 
        {
            OnSelectedListItemChanged(value);
            this.RaiseAndSetIfChanged( ref _selectedListItem, value);
        }

    }

    private ViewModelBase _currentPage = new HomePageViewModel();

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;

        var vm = Design.IsDesignMode
            ? Activator.CreateInstance(value.ModelType)
            : Locator.Current.GetService(value.ModelType);

        if (vm is not ViewModelBase vmb)
        {
            this.Log().Error("ViewModel not found");
            return;
        }

        CurrentPage = vmb;
    }

    public ObservableCollection<ListItemTemplate> Items { get; }

    public ICommand TriggerPaneCommand { get; }
}
