// Copyright 2024 (c) IOExcept10n (contact https://github.com/IOExcept10n)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using AnimaFiltering.Services;
using AnimaFiltering.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace AnimaFiltering.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        if (!Design.IsDesignMode)
        {
            DataContext = App.Services.GetRequiredService<MainViewModel>();
        }
        AddHandler(DragDrop.DropEvent, Drop);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        CameraSelector.SelectedIndex = 0;
    }

    /// <summary>
    /// Handles drag and drop event.
    /// </summary>
    /// <remarks>
    /// Occurs when user drops directory on program.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Drop(object? sender, DragEventArgs e)
    {
        if (DataContext is not MainViewModel mvm) return;
        StartHandleButton.IsEnabled = false;
        mvm.Attachments.Clear();
        var files = e.Data.GetFiles();
        bool flag = false;
        if (files != null)
        {
            foreach (var file in files)
            {
                if (Directory.Exists(file.Path.LocalPath))
                {
                    mvm.Attachments.Add(new(file));
                    flag = true;
                    break;
                }
            }
        }
        if (flag)
        {
            StartHandleButton.IsEnabled = true;
        }
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        StartHandleButton.IsEnabled = false;
        if (DataContext is not MainViewModel mvm)
            return;
        var options = App.Services.GetRequiredService<AppPreferences>();
        int minWidth = (int)(XSize.Value ?? 128);
        int minHeight = (int)(YSize.Value ?? 128);
        options.MinWidth = minWidth;
        options.MinHeight = minHeight;
        var camera = (CameraStats)CameraSelector.SelectedItem!;
        var progress = new Progress<int>();
        string dirPath = mvm.Attachments[0].FilePath.Path.LocalPath;
        ProgressDisplay.Maximum = Directory.EnumerateFiles(dirPath, "*.jpg").Count();
        ProgressDisplay.Minimum = 0;
        progress.ProgressChanged += Progress_ProgressChanged;
        var results = App.Services.GetRequiredService<AnimalFilteringService>().FilterAnimalsAsync(dirPath);
        await App.Services.GetRequiredService<ReportBuilder>().WriteToCsvAsync(results, camera, "../Report.csv", progress);
        App.Services.GetRequiredService<CameraManager>().Save();
        ProgressStep.Content = "Completed!";
        ProgressDisplay.Value = 0;
        StartHandleButton.IsEnabled = true;
    }

    private void Progress_ProgressChanged(object? sender, int e)
    {
        int step = Math.Clamp(e, (int)ProgressDisplay.Minimum, (int)ProgressDisplay.Maximum);
        ProgressDisplay.Value = step;
        ProgressStep.Content = $"{e}/{Math.Max(ProgressDisplay.Maximum, e)}";
    }
}