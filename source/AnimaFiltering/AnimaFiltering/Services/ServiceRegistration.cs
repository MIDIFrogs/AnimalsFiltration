// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using AnimaFiltering.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AnimaFiltering.Services
{
    /// <summary>
    /// Represents an utility to register app services.
    /// </summary>
    internal static class ServiceRegistration
    {
        private const string OptionsFileName = "Options.json";
        private const string CamerasFileName = "Cameras.json";

        /// <summary>
        /// Adds application services to the DI container.
        /// </summary>
        /// <param name="services">Services collection to add.</param>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddOptions()
                .AddFilters()
                .AddViewModels()
                .AddSingleton<YoloProvider>();
        }

        /// <summary>
        /// Adds app view models to the DI container.
        /// </summary>
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services
                .AddTransient<StatsViewModel>()
                .AddTransient<MainViewModel>();
        }

        /// <summary>
        /// Adds image filters to the DI container.
        /// </summary>
        public static IServiceCollection AddFilters(this IServiceCollection services)
        {
            return services
                .AddSingleton<UserFilters>()
                .AddSingleton<AnimalFilteringService>()
                .AddSingleton<CameraStats>()
                .AddSingleton<ReportBuilder>();
        }

        /// <summary>
        /// Adds app options to the DI container.
        /// </summary>
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