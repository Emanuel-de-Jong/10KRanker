using Discord.WebSocket;
using System.Threading.Tasks;

namespace _10KRanker
{
    public class UnitTest
    {
        public static bool Testing { get; } = false;
        private const string newline = "‎\n";

        private SocketTextChannel channel;

        private async Task Send(string message)
        {
            await channel.SendMessageAsync("!" + message);
            await Task.Delay(2000);
        }

        private async Task SendTitle(string message, bool withNewline = false)
        {
            await channel.SendMessageAsync((withNewline ? newline : "") + $"=={ message }==");
        }

        public async Task Test(DiscordSocketClient client)
        {
            channel = client.GetGuild(Secrets.TestServerId).GetTextChannel(Secrets.TestChannelId);

            await TestAddMap(true);
            await TestRemoveMap(true);
            await TestAddNominator(true);
            await TestRemoveNominator(true);
            await TestUpdateMapStatus(true);

            await channel.SendMessageAsync(newline + newline);
            await Send("list");
        }

        private async Task TestAddMap(bool testBadWeather)
        {
            await SendTitle("ADD MAP", true);

            //add1 <id>
            await Send("add 1509186");
            //add2 <link>
            await Send("add https://osu.ppy.sh/beatmapsets/1466367#mania/3011461");
            //add3 <id> <status>
            await Send("add 1343787 map3 status");
            //add4 <id>
            await Send("add 1095022");
            //add5 <id> <status>
            await Send("add 1509353 map5 %$#@test");

            if (testBadWeather)
            {
                await SendTitle("BAD WEATHER");

                //add <link id no digit>
                await Send("add osu.ppy.sh/beatmapsets/11X3846");
                //add <invalid link>
                await Send("add osu.ppy.sh/bla");
                //add <wrong name>
                await Send("add abc123test321cba");
                //add <wrong id>
                await Send("add 99999999");
                //add <id of std>
                await Send("add 1393811");
                //add <id of other k>
                await Send("add 1369976");
                //add <id of ranked>
                await Send("add 1193846");

                //add1 <same id>
                await Send("add 1509186");
                //add1 <name same map>
                await Send("add \"10K Indo Pack #2\"");
            }
        }

        private async Task TestRemoveMap(bool testBadWeather)
        {
            await SendTitle("REMOVE MAP", true);

            //rm4 <id>
            await Send("rm 1095022");

            if (testBadWeather)
            {
                await SendTitle("BAD WEATHER");

                //rm4 <same id>
                await Send("rm 1095022");
            }
        }

        private async Task TestAddNominator(bool testBadWeather)
        {
            await SendTitle("ADD NOMINATOR", true);

            //addbn1 <id>1 <id>
            await Send("addbn 1509186 4815717");
            //addbn2 <id>1 <name>
            await Send("addbn 1509186 Unpredictable");
            //addbn3 <id>2 <link with />
            await Send("addbn 1466367 osu.ppy.sh/users/259972/fruits");
            //addbn4 <id>3 <id>
            await Send("addbn 1343787 2225008");

            if (testBadWeather)
            {
                await SendTitle("BAD WEATHER");

                //addbn <id>1 <invalid link>
                await Send("addbn 1509186 osu.ppy.sh/bla");
                //addbn <id>1 <wrong name>
                await Send("addbn 1509186 abc123test321cba");
                //addbn <id>1 <wrong id>
                await Send("addbn 1509186 99999999");

                //addbn1 <id>1 <same id>
                await Send("addbn 1509186 4815717");
                //addbn1 <id>1 <name same bn>
                await Send("addbn 1509186 feerum");
            }
        }

        private async Task TestRemoveNominator(bool testBadWeather)
        {
            await SendTitle("REMOVE NOMINATOR", true);

            //rmbn4 <name>3 <id>
            await Send("rmbn \"lalabai call me\" 2225008");

            if (testBadWeather)
            {
                await SendTitle("BAD WEATHER");

                //rmbn4 <name>3 <same id>
                await Send("rmbn \"lalabai call me\" 2225008");
            }
        }

        private async Task TestUpdateMapStatus(bool testBadWeather)
        {
            await SendTitle("UPDATE MAP STATUS", true);

            //cs <id>2 <status>
            await Send("cs 1466367 map2 status");
            //cs <id>3 <status>
            await Send("cs 1343787 map3 status edit1");
            //cs <id>3 <other status>
            await Send("cs 1343787 map3 status edit2");

            if (testBadWeather)
            {
                //await SendTitle("BAD WEATHER");
            }
        }
    }
}
