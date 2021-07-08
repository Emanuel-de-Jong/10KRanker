using _10KRanker.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace _10KRanker
{
    public class Program
    {
        private Timer updateDBTablesTimer;
        private DiscordSocketClient client;

        private static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using ServiceProvider services = ConfigureServices();

            client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;

            await client.LoginAsync(TokenType.Bot, Secrets.DiscordToken);
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            client.Ready += StartTestOnClientReady;

            updateDBTablesTimer = new Timer(1 * 24 * 60 * 60 * 1000);
            updateDBTablesTimer.AutoReset = true;
            updateDBTablesTimer.Elapsed += new ElapsedEventHandler(OsuToDB.OnUpdateDBTablesTimerElapsed);
            updateDBTablesTimer.Start();
            //OsuToDB.OnUpdateDBTablesTimerElapsed(null, null);

            await Task.Delay(-1);
        }

        private async Task StartTestOnClientReady()
        {
            if (UnitTest.Testing)
                new UnitTest().Test(client);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

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
