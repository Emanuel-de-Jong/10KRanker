using Database;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10KRanker
{
    public class UnitTest
    {
        public static bool Testing { get; } = true;

        private SocketTextChannel channel;

        public async Task Test(DiscordSocketClient client)
        {
            DB.ClearDatabase();

            channel = client.GetGuild(Secrets.TestServerId).GetTextChannel(Secrets.TestChannelId);

            //add
            await Send("add");
            //add <link id no digit>		osu.ppy.sh/beatmapsets/11X3846
            await Send("add osu.ppy.sh/beatmapsets/11X3846");
            //add <invalid link>			osu.ppy.sh/bla
            await Send("add osu.ppy.sh/bla");
            //add <wrong name>				test
            await Send("add test");
            //add <wrong id>				99999999
            await Send("add 99999999");
            //add <id of std>				1393811
            await Send("add 1393811");
            //add <id of other k>			1369976
            await Send("add 1369976");
            //add1 <id>						1193846
            await Send("add 1193846");
            //add1 <same id>				1193846
            await Send("add 1193846");
            //add1 <name same map>			"BLACK or WHITE?"
            await Send("add \"BLACK or WHITE ?\"");
            //add2 <link>					https://osu.ppy.sh/beatmapsets/1466367#mania/3011461
            await Send("add https://osu.ppy.sh/beatmapsets/1466367#mania/3011461");
            //add3 <id> <status>			1343787 map3 status
            await Send("add 1343787 map3 status");
            //add4 <id>						1095022
            await Send("add 1095022");

            //rm4 <id>						1095022
            //rm4 <same id>					1095022

            //addbn <id>1 <invalid link>	osu.ppy.sh/bla
            //addbn <id>1 <wrong name>		test
            //addbn <id>1 <wrong id>		99999999
            //addbn1 <id>1 <id>				4815717
            //addbn1 <id>1 <same id>		4815717
            //addbn1 <id>1 <name same bn>	Feerum
            //addbn2 <id>1 <name>			Unpredictable
            //addbn3 <id>2 <link with />	osu.ppy.sh/users/259972/fruits
            //addbn4 <id>3 <id>				2225008

            //rmbn4 <name>3 <id>			"lalabai call me" 2225008
            //rmbn4 <name>3 <same id>		"lalabai call me" 2225008

            //cs <id>2 <status>				1466367 map2 status
            //cs <id>3 <status>				1343787 map3 status edit1
            //cs <id>3 <other status>		1343787 map3 status edit2

            //list

            //help
        }

        private async Task Send(string message)
        {
            await channel.SendMessageAsync("!" + message);
            await Task.Delay(1500);
        }

        private string expectedResult =
@"
!add
error: BadArgCount: The input text has too few parameters.
!add osu.ppy.sh/beatmapsets/11X3846
error: Exception: The id is not a number
!add osu.ppy.sh/bla
error: Exception: The map link is not valid
!add test
error: Exception: The key value at position 0 of the call to 'DbSet<Map>.Find' was of type 'string', which does not match the property type of 'long'.
!add 99999999
error: Exception: A beatmapset with that id does not exist
!add 1393811
error: Exception: The beatmapset does not have a 10K difficulty
!add 1369976
error: Exception: The beatmapset does not have a 10K difficulty
!add 1193846
error: Exception: The map already exists
!add 1193846
error: Exception: The map already exists
!add ""BLACK or WHITE?""
error: Exception: The key value at position 0 of the call to 'DbSet<Map>.Find' was of type 'string', which does not match the property type of 'long'.
!add https://osu.ppy.sh/beatmapsets/1466367#mania/3011461
error: Exception: An error occurred while updating the entries.See the inner exception for details.
!add 1343787 map3 status
error: Exception: An error occurred while updating the entries.See the inner exception for details.
!add 1095022
error: Exception: An error occurred while updating the entries.See the inner exception for details.
";
    }
}
