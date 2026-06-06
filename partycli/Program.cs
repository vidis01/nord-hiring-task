using partycli.Commands;
using partycli.Infrastructure;
using PartyCli.Configuration;
using System.CommandLine;

namespace partycli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = ConfigurationLoader.Build();
            var container = DependencyContainer.Build(configuration);
            var rootCommand = new RootCommand("partycli — Information about servers status tool");

            CommandRegistry.Register(rootCommand, configuration, container);

            rootCommand.Parse(args).Invoke();
        }
    }
}
