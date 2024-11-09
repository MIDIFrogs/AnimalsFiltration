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
        [ObservableProperty] private DonutChart chart = new() 
        {
            Entries = 
            [
                new() 
                { 
                    Value = 1, 
                    Label = "Выберите камеру", 
                    ValueLabel = "--",
                    Color = new SKColor(0, 0, 0, 0x40),
                }
            ],
            BackgroundColor = SKColor.Empty,
        };

        public CameraManager Cameras { get; } = App.Services.GetRequiredService<CameraManager>();

        public void OnSelectionChanged(int newIndex)
        {
            SelectedStats = Cameras[newIndex];
            Chart = new DonutChart()
            {
                Entries = [
                    new(){
                        Color = new SKColor(0x55, 0xff, 0x55, 0x70),
                        Label = "Хорошие фото",
                        ValueLabel = SelectedStats.GoodImages.ToString(),
                        Value = SelectedStats.GoodImages,
                        TextColor = SKColors.Black,
                    },
                    new(){
                        Color = new SKColor(0xff, 0x55, 0x55, 0x70),
                        Label = "Плохие фото",
                        ValueLabel = (SelectedStats.ProcessedImages - SelectedStats.GoodImages).ToString(),
                        Value = SelectedStats.ProcessedImages - SelectedStats.GoodImages,
                        TextColor = SKColors.Black
                    }
                ],
                BackgroundColor = SKColor.Empty,
                Margin = 30
            };
        }
    }
}