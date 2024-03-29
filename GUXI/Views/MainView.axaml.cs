﻿using System;
using System.Collections.Generic;

using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Interactivity;

using FluentAvalonia.UI.Controls;

using GUXI.Services;
using GUXI.Views.Dialogs;

namespace GUXI.Views
{
public partial class MainView : ViewBase
{
    private Dictionary<string, UserControl> _views = [];
    private int _NVSelectedIndexLast = 0;

    public MainView()
    {
        InitializeComponent();

        var navigationView = this.FindControl<NavigationView>("navigationView");
        if (navigationView == null)
        {
            return;
        }

        navigationView.SelectionChanged += OnNavigationViewSelectionChanged;
        navigationView.SelectedItem = navigationView.MenuItems[_NVSelectedIndexLast];
    }

    private async void OnNavigationViewSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (sender is not NavigationView navigationView || e.SelectedItem is not NavigationViewItem item)
        {
            return;
        }

        if (item.Tag!.ToString() == "About")
        {
            // TODO: can we remove this workaround?
            // select back the last item outside this method so the property changed will propagate
            // https://github.com/amwx/FluentAvalonia/discussions/501
            await Dispatcher.UIThread.InvokeAsync(() => navigationView.SelectedItem =
                                                      navigationView.MenuItems[_NVSelectedIndexLast]);

            await new AboutDialog(this).Show();
        }
        else
        {
            navigationView.Content = CreateView(item);
        }

        _NVSelectedIndexLast = navigationView.MenuItems.IndexOf(e.SelectedItem);
    }

    private UserControl CreateView(NavigationViewItem item)
    {
        // search the view cache first
        var viewTypeName = $"GUXI.Views.{item.Tag}";
        if (_views.TryGetValue(viewTypeName, out UserControl? value))
        {
            return value;
        }

        // create the view model
        var viewModelTypeName = $"GUXI.ViewModels.{item.Tag}Model";
        var viewModelType =
            Type.GetType(viewModelTypeName) ?? throw new TypeAccessException($"Type not found: {viewModelTypeName}");
        var viewModel = DI.Create(viewModelType);

        // create the view
        var viewType = Type.GetType(viewTypeName) ?? throw new TypeAccessException($"Type not found: {viewTypeName}");
        var view = DI.Create(viewType);
        if (view is not ViewBase viewBase)
        {
            throw new InvalidCastException($"Type not derived from UserControl: {viewTypeName}");
        }

        viewBase.DataContext = viewModel;

        // add the view to our cache
        _views.Add(viewTypeName, viewBase);

        viewBase.Initialize();
        return viewBase;
    }

    private void OnGUXILoaded(object? sender, RoutedEventArgs e) => DI.Initialize();
    private void OnGUXIUnloaded(object? sender, RoutedEventArgs e) => DI.Uninitialize();
}
}
