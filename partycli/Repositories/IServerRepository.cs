using partycli.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace partycli.Repositories
{
    public interface IServerRepository
    {
        Task<List<VpnServer>> GetAllServersListAsync();
        Task<List<VpnServer>> GetAllServerByCountryListAsync(int countryId);
        Task<List<VpnServer>> GetAllServerByProtocolListAsync(int protocol);
    }
}