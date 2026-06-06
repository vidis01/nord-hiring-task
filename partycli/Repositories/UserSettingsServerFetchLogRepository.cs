using partycli.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace partycli.Repositories
{
    public class UserSettingsServerFetchLogRepository : IServerFetchLogRepository
    {
        public Task AppendAsync(ServerFetchLog entry)
        {
            // Load existing log entries first
            var entries = LoadEntries();

            entries.Add(entry);

            Properties.Settings.Default.log =
                JsonSerializer.Serialize(entries);

            Properties.Settings.Default.Save();

            return Task.CompletedTask;
        }

        public Task<List<ServerFetchLog>> LoadAllAsync()
        {
            return Task.FromResult(LoadEntries());
        }

        public Task ClearAsync()
        {
            Properties.Settings.Default.log = string.Empty;
            Properties.Settings.Default.Save();

            return Task.CompletedTask;
        }

        // Private helper — reused by both AppendAsync and LoadAllAsync
        private List<ServerFetchLog> LoadEntries()
        {
            string json = Properties.Settings.Default.log;

            if (string.IsNullOrWhiteSpace(json))
                return new List<ServerFetchLog>();

            return JsonSerializer.Deserialize<List<ServerFetchLog>>(json)
                   ?? new List<ServerFetchLog>();
        }
    }
}
