using partycli.Models;
using System;
using System.Collections.Generic;

namespace partycli.Display
{
    public class TableServerDisplay : IServerDisplay
    {
        public void Show(List<ServerModel> servers)
        {
            if (servers.Count == 0)
            {
                Console.WriteLine("No servers found.");
                return;
            }

            Console.WriteLine($"{"Name",-30} {"Load",-12} {"Status"}");
            Console.WriteLine(new string('-', 50));

            foreach (var s in servers)
            {
                Console.WriteLine($"{s.Name,-30} {s.Load,-12} {s.Status}");
            }

            Console.WriteLine($"Total servers: {servers.Count}");
        }
    }
}
