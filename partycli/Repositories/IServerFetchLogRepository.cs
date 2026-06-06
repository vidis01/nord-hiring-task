using partycli.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace partycli.Repositories
{
    public interface IServerFetchLogRepository
    {
        Task AppendAsync(ServerFetchLog entry);
        Task<List<ServerFetchLog>> LoadAllAsync();
        Task ClearAsync();
    }
}