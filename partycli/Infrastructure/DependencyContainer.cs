using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using partycli.Commands;
using partycli.Configuration;
using partycli.Display;
using partycli.Repositories;
using System.Net.Http;

namespace partycli.Infrastructure
{
    public static class DependencyContainer
    {
        public static ServiceProvider Build(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            // Config
            var apiSettings = new ApiSettings();
            configuration.GetSection("Api").Bind(apiSettings);
            services.AddSingleton(apiSettings);

            // HTTP
            services.AddSingleton<HttpClient>();

            // Remote repository - responsible for fetching data from API
            services.AddTransient<IServerRepository, ServerRepository>();

            // Local repository — saves/loads servers info to/from user settings
            services.AddTransient<ILocalServerRepository, UserSettingsLocalServerRepository>();

            // Fetch log repository - logs fetched data to user settings
            services.AddTransient<IServerFetchLogRepository, UserSettingsServerFetchLogRepository>();

            // Display to console
            services.AddTransient<IServerDisplay, TableServerDisplay>();

            // server_list command logic
            services.AddTransient<ServerListCommand>();

            return services.BuildServiceProvider();
        }
    }
}
