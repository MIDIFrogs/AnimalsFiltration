using AnimaFiltering.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace AnimaFiltering.Views;

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

    protected override void OnInitialized()
    {
        base.OnInitialized();
        CameraSelector.SelectedIndex = 0;
    }

    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        (DataContext as StatsViewModel)?.OnSelectionChanged(CameraSelector.SelectedIndex);
    }
}