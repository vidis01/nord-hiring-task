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

        public ServerRepository(HttpClient httpClient, ApiSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<List<ServerModel>> GetAllServersListAsync()
        {
            return await GetAsync(_settings.ServerListUrl);
        }

        public async Task<List<ServerModel>> GetAllServerByCountryListAsync(int countryId)
        {
            string url = $"{_settings.ServerListByCountryUrl}{countryId}";
            return await GetAsync(url);
        }

        public async Task<List<ServerModel>> GetAllServerByProtocolListAsync(int protocol)
        {
            string url = $"{_settings.ServerListByProtocolUrl}{protocol}";
            return await GetAsync(url);
        }

        // Single private method that does the actual HTTP call
        private async Task<List<ServerModel>> GetAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<ServerModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<ServerModel>();
        }
    }
}
