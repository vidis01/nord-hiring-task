using partycli.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace partycli.Repositories
{
    public interface IServerRepository
    {
        Task<List<ServerModel>> GetAllServersListAsync();
        Task<List<ServerModel>> GetAllServerByCountryListAsync(int countryId);
        Task<List<ServerModel>> GetAllServerByProtocolListAsync(int protocol);
    }
}
