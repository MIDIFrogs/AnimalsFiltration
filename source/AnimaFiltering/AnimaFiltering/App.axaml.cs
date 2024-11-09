using AnimaFiltering.Services;
using AnimaFiltering.ViewModels;
using AnimaFiltering.Views;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Globalization;

namespace AnimaFiltering;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        AvaloniaXamlLoader.Load(this);
    }

    public override void RegisterServices()
    {
        base.RegisterServices();
        var services = new ServiceCollection().AddServices();
        Services = services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

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
        //var f = Services.GetRequiredService<AnimalFilteringService>().FilterAnimalsAsync("H:\\HackTon\\MejNar 2024\\train_data_minprirodi\\images");
        //await Services.GetRequiredService<ReportBuilder>().WriteToCsvAsync(f, "report.csv");
        base.OnFrameworkInitializationCompleted();
    }
}
