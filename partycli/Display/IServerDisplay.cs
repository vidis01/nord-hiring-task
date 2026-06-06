using partycli.Models;
using System.Collections.Generic;

namespace partycli.Display
{
    public interface IServerDisplay
    {
        void Show(List<ServerModel> servers);
    }
}
