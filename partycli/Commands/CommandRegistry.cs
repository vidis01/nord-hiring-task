using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using partycli.Models;
using System;
using System.Collections.Generic;
using System.CommandLine;

namespace partycli.Commands
{
    public static class CommandRegistry
    {
        public static void Register(RootCommand rootCommand, IConfiguration configuration, ServiceProvider container)
        {
            var commands = configuration
                .GetSection("Commands")
                .Get<Dictionary<string, CommandSettings>>();

            if (commands == null) return;

            foreach (var command in commands)
            {
                Command cmd = null;

                switch (command.Key)
                {
                    case "server_list":
                        cmd = container.GetRequiredService<ServerListCommand>().Build(command.Value);
                        break;
                    case "config":
                        break;
                    default:
                        Console.WriteLine("Command not implemented.");
                        break;
                }

                if (cmd != null)
                    rootCommand.Subcommands.Add(cmd);
            }
        }
    }
}
