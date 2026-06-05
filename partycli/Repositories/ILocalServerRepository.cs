using partycli.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace partycli.Repositories
{
    public interface ILocalServerRepository
    {
        Task SaveAsync(List<VpnServer> servers);
        Task<List<VpnServer>> LoadAsync();
        Task ClearAsync();
    }
}