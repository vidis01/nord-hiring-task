using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using partycli.Commands;
using partycli.Configuration;
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

            // Remote repository
            services.AddTransient<IServerRepository, ServerRepository>();

            // Local repository — swap this one line to change persistence mechanism
            services.AddTransient<ILocalServerRepository, UserSettingsLocalServerRepository>();

            // Commands
            services.AddTransient<ServerListCommand>();

            return services.BuildServiceProvider();
        }
    }
}
