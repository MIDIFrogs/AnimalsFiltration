using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimaFiltering.Services.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AnimaFiltering.Services
{
    internal static class ServiceRegistration
    {
        private const string OptionsFileName = "Options.json";

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddOptions()
                .AddFilters()
                .AddSingleton<YoloProvider>();
        }

        public static IServiceCollection AddFilters(this IServiceCollection services) 
        {
            return services.AddSingleton<UserFilters>().AddSingleton<AnimalFilteringService>(); 
        }

        public static IServiceCollection AddOptions(this IServiceCollection services)
        {
            AppPreferences options = AppPreferences.LoadOrCreate(OptionsFileName);
            services.AddSingleton(options);
            return services;
        }
    }
}
