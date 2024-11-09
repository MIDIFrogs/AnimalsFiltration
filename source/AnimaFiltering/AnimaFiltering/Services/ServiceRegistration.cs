// Copyright 2024 (c) IOExcept10n (contact https://github.com/IOExcept10n)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using AnimaFiltering.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AnimaFiltering.Services
{
    internal static class ServiceRegistration
    {
        private const string OptionsFileName = "Options.json";
        private const string CamerasFileName = "Cameras.json";

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddOptions()
                .AddFilters()
                .AddViewModels()
                .AddSingleton<YoloProvider>();
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services
                .AddTransient<StatsViewModel>()
                .AddTransient<MainViewModel>();
        }

        public static IServiceCollection AddFilters(this IServiceCollection services)
        {
            return services
                .AddSingleton<UserFilters>()
                .AddSingleton<AnimalFilteringService>()
                .AddSingleton<CameraStats>()
                .AddSingleton<ReportBuilder>();
        }

        public static IServiceCollection AddOptions(this IServiceCollection services)
        {
            AppPreferences options = AppPreferences.LoadOrCreate(OptionsFileName);
            CameraManager manager = CameraManager.LoadOrCreate(CamerasFileName);
            services.AddSingleton(options);
            services.AddSingleton(manager);
            return services;
        }
    }
}