using System;
using System.Collections.Generic;

namespace partycli.Models
{
    public class ServerFetchLog
    {
        public DateTime FetchedAt { get; set; }
        public string Command { get; set; }  // "server_list --france"
        public List<ServerModel> Servers { get; set; } = new List<ServerModel>();
    }
}
