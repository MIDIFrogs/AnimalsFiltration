using AnimaFiltering.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace AnimaFiltering;

public partial class StatsView : UserControl
{
    public StatsView()
    {
        InitializeComponent();
        if (!Design.IsDesignMode)
        {
            DataContext = App.Services.GetRequiredService<StatsViewModel>();
        }
    }

    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        (DataContext as StatsViewModel)?.OnSelectionChanged(CameraSelector.SelectedIndex);
    }
}