using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using _10KRanker.Services;
using OsuAPI;
using Database;
using System.Timers;

namespace _10KRanker
{
    public class Program
    {
        private DiscordSocketClient client;

        static void Main(string[] args)  => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                client = services.GetRequiredService<DiscordSocketClient>();

                client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                await client.LoginAsync(TokenType.Bot, Secrets.DiscordToken);
                await client.StartAsync();

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                client.Ready += OnClientReady;

                await Task.Delay(-1);
            }
        }

        private async Task OnClientReady()
        {
            if (UnitTest.Testing)
                await new UnitTest().Test(client);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
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
