using partycli.Display;
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
        private const string _commandName = "server_list";
        private readonly IServerRepository _repository;
        private readonly ILocalServerRepository _localRepository;
        private readonly IServerFetchLogRepository _logRepository;
        private readonly IServerDisplay _display;

        public ServerListCommand(
            IServerRepository repository, 
            ILocalServerRepository localRepository, 
            IServerFetchLogRepository logRepository,
            IServerDisplay serverDisplay)
        {
            _repository = repository;
            _localRepository = localRepository;
            _logRepository = logRepository;
            _display = serverDisplay;
        }

        public Command Build(CommandSettings settings)
        {
            var command = new Command(_commandName, settings.Description);
            var optionMap = BuildOptions(command, settings.Params);

            command.SetAction(async (parseResult) =>
            {
                var param = settings.Params.FirstOrDefault(p =>
                    optionMap.TryGetValue(p.Flag, out var opt) &&
                    parseResult.GetValue(opt) == true);

                List<ServerModel> servers;

                if (param != null && param.Flag == "--local")
                {
                    servers = await _localRepository.LoadAsync();
                    _display.Show(servers);

                    return;
                }
                
                string commandLabel;

                if (param == null)
                {
                    servers = await _repository.GetAllServersListAsync();
                    commandLabel = _commandName; ;
                }
                else
                {
                    servers = await FetchFilteredAsync(param);
                    commandLabel = $"{_commandName} {param.Flag}";
                }

                if (servers == null) return;

                await _localRepository.SaveAsync(servers);

                await _logRepository.AppendAsync(new ServerFetchLog
                {
                    FetchedAt = DateTime.Now,
                    Command = commandLabel,
                    Servers = servers
                });

                _display.Show(servers);
            });

            return command;
        }

        private async Task<List<ServerModel>> FetchFilteredAsync(ParamEntry param)
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

        private static Dictionary<string, Option<bool>> BuildOptions(Command command, List<ParamEntry> paramEntries)
        {
            var map = new Dictionary<string, Option<bool>>();

            foreach (var param in paramEntries)
            {
                var option = new Option<bool>(param.Flag);
                command.Options.Add(option);
                map[param.Flag] = option;
            }

            return map;
        }
    }
}
