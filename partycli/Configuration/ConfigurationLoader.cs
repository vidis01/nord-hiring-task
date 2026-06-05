using System;
using Microsoft.Extensions.Configuration;

namespace PartyCli.Configuration
{
    public static class ConfigurationLoader
    {
        public static IConfiguration Build()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile("commands.json", optional: false, reloadOnChange: false)
                .Build();
        }
    }
}
