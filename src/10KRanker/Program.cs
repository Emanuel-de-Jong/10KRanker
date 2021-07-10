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
        private System.Timers.Timer updateDBTablesTimer;
        private DiscordSocketClient client;
        private Log log;

        private static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            log = new Log("console");

            using ServiceProvider services = ConfigureServices();

            client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;

            await client.LoginAsync(TokenType.Bot, Secrets.DiscordToken);
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            client.Ready += OnClientReady;

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

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<PictureService>()
                .BuildServiceProvider();
        }
    }
}
