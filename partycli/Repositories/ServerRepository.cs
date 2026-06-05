using Microsoft.Extensions.Options;
using partycli.Configuration;
using partycli.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace partycli.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _settings;

        // Dependencies injected — never created inside
        public ServerRepository(HttpClient httpClient, ApiSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<List<VpnServer>> GetAllServersListAsync()
        {
            return await GetAsync(_settings.ServerListUrl);
        }

        public async Task<List<VpnServer>> GetAllServerByCountryListAsync(int countryId)
        {
            string url = $"{_settings.ServerListByCountryUrl}{countryId}";
            return await GetAsync(url);
        }

        public async Task<List<VpnServer>> GetAllServerByProtocolListAsync(int protocol)
        {
            string url = $"{_settings.ServerListByProtocolUrl}{protocol}";
            return await GetAsync(url);
        }

        // Single private method that does the actual HTTP call
        // Only one place to change if HTTP logic changes
        private async Task<List<VpnServer>> GetAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<VpnServer>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<VpnServer>();
        }
    }
}