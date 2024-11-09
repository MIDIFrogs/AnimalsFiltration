// Copyright 2024 (c) IOExcept10n (contact https://github.com/IOExcept10n)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using AnimaFiltering.Services;
using Avalonia.Microcharts;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;

namespace AnimaFiltering.ViewModels
{
    internal partial class StatsViewModel : ViewModelBase
    {
        [ObservableProperty] private CameraStats? selectedStats;
        [ObservableProperty] private DonutChart? chart;

        public CameraManager Cameras { get; } = App.Services.GetRequiredService<CameraManager>();

        public void OnSelectionChanged(int newIndex)
        {
            SelectedStats = Cameras[newIndex];
            Chart = new DonutChart()
            {
                Entries = [
                    new(){
                        Color = SKColors.DarkGreen,
                        Label = "Хорошие фото",
                        ValueLabel = SelectedStats.GoodImages.ToString(),
                        Value = SelectedStats.GoodImages
                    },
                    new(){
                        Color = SKColors.DarkRed,
                        Label = "Плохие фото",
                        ValueLabel = (SelectedStats.ProcessedImages - SelectedStats.GoodImages).ToString(),
                        Value = SelectedStats.ProcessedImages - SelectedStats.GoodImages
                    }
                ]
            };
        }
    }
}