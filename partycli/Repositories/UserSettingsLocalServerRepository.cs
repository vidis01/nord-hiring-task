using partycli.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace partycli.Repositories
{
    public class UserSettingsLocalServerRepository : ILocalServerRepository
    {
        public Task SaveAsync(List<VpnServer> servers)
        {
            string json = JsonSerializer.Serialize(servers);

            var settings = Properties.Settings.Default;
            settings.serverlist = json;
            settings.Save();

            return Task.CompletedTask;
        }

        public Task<List<VpnServer>> LoadAsync()
        {
            string json = Properties.Settings.Default.serverlist;

            if (string.IsNullOrWhiteSpace(json))
                return Task.FromResult(new List<VpnServer>());

            var servers = JsonSerializer.Deserialize<List<VpnServer>>(json)
                          ?? new List<VpnServer>();

            return Task.FromResult(servers);
        }

        public Task ClearAsync()
        {
            Properties.Settings.Default.serverlist = string.Empty;
            Properties.Settings.Default.Save();

            return Task.CompletedTask;
        }
    }
}