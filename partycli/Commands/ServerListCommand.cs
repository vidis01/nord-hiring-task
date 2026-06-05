using partycli.Models;
using partycli.Repositories;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;

namespace partycli.Commands
{
    public class ServerListCommand
    {
        private readonly IServerRepository _repository;
        private readonly ILocalServerRepository _localRepository;

        // IServerRepository injected — never newed up inside
        public ServerListCommand(IServerRepository repository, ILocalServerRepository localRepository)
        {
            _repository = repository;
            _localRepository = localRepository;
        }

        public Command Build(CommandSettings settings)
        {
            var command = new Command("server_list", settings.Description);
            var optionMap = BuildOptions(command, settings.Params);

            command.SetAction(async (parseResult) =>
            {
                var param = settings.Params.FirstOrDefault(p =>
                    optionMap.TryGetValue(p.Flag, out var opt) &&
                    parseResult.GetValue(opt) == true);

                List<VpnServer> servers;

                if (param == null)
                    servers = await _repository.GetAllServersListAsync();
                else
                    servers = await FetchFilteredAsync(param);

                if (servers == null) return;

                await _localRepository.SaveAsync(servers);

                PrintServers(servers);
            });

            return command;
        }

        private async Task ExecuteAllAsync()
        {
            var servers = await _repository.GetAllServersListAsync();
            PrintServers(servers);
        }

        private async Task<List<VpnServer>> FetchFilteredAsync(ParamEntry param)
        {
            switch (param.Type)
            {
                case "region":
                    return await _repository.GetAllServerByCountryListAsync(param.Code);

                case "connection":
                    return await _repository.GetAllServerByProtocolListAsync(param.Code);

                default:
                    Console.WriteLine($"Unknown param type: {param.Type}");
                    return null;
            }
        }

        private static void PrintServers(List<VpnServer> servers)
        {
            if (servers.Count == 0)
            {
                Console.WriteLine("No servers found.");
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Name",-30} {"Country",-15} {"Protocol",-12} {"Status"}");
            Console.WriteLine(new string('-', 75));

            foreach (var s in servers)
                Console.WriteLine($"{s.Id,-5} {s.Name,-30} {s.Country,-15} {s.Protocol,-12} {s.Status}");
        }

        private static Dictionary<string, Option<bool>> BuildOptions(Command command, List<ParamEntry> paramEntries)
        {
            var map = new Dictionary<string, Option<bool>>();

            foreach (var param in paramEntries)
            {
                var option = new Option<bool>(param.Flag, $"Target:{param.Flag.TrimStart('-')}");
                command.Options.Add(option);
                map[param.Flag] = option;
            }

            return map;
        }
    }
}