using _10KRanker.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Logger;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace _10KRanker
{
    public class Program
    {
        private DiscordSocketClient client;
        private CommandService commandService;

        private System.Timers.Timer updateDBTablesTimer;
        private Log log;

        private static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            log = new Log("console");

            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                ExclusiveBulkDelete = false,
            });

            commandService = new CommandService(new CommandServiceConfig()
            {

            });

            client.Log += LogAsync;
            client.Ready += OnClientReady;
            commandService.Log += LogAsync;

            using ServiceProvider services = ConfigureServices();

            await client.LoginAsync(TokenType.Bot, Secrets.DiscordToken);
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(-1);
        }

        private async Task OnClientReady()
        {
            _ = Task.Run(() =>
            {
                Thread.Sleep(3000);

                updateDBTablesTimer = new System.Timers.Timer(1 * 24 * 60 * 60 * 1000);
                updateDBTablesTimer.AutoReset = true;
                updateDBTablesTimer.Elapsed += OsuToDB.OnUpdateDBTablesTimerElapsed;
                updateDBTablesTimer.Start();

                //OsuToDB.OnUpdateDBTablesTimerElapsed(null, null);

                if (UnitTest.Testing)
                    _ = new UnitTest().Test(client);
            });

        }

        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.ToString());

            log.Write(message.ToString());

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>(client)
                .AddSingleton<CommandService>(commandService)
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<PictureService>()
                .BuildServiceProvider();
        }
    }
}
