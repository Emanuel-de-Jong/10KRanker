using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using _10KRanker.Services;
using OsuAPI;
using OsuSharp;
using Database;
using System.Collections.Generic;

namespace _10KRanker.Modules
{
    public class MainModule : ModuleBase<SocketCommandContext>
    {
        [Command("add")]
        [Alias("addmap", "create", "createmap")]
        public async Task AddMapAsync(string mapIndicator, [Remainder] string status = null)
        {
            bool mapExists = false;
            Map map = null;
            try
            {
                map = Validator.InputToMap(mapIndicator, out mapExists);
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }

            if (mapExists)
            {
                await ReplyAsync("The map already exists");
                return;
            }

            map.Status = status;
            DB.Add(map);

            await ReplyAsync("Map added");
        }


        [Command("rm")]
        [Alias("rmmap", "remove", "removemap", "delete", "deletemap")]
        public async Task RemoveMapAsync(string mapIndicator)
        {
            bool mapExists = false;
            Map map = null;
            try
            {
                map = Validator.InputToMap(mapIndicator, out mapExists);
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }

            if (!mapExists)
            {
                await ReplyAsync("The map doesn't exist");
                return;
            }

            DB.Remove(map);

            await ReplyAsync("Map removed");
        }




        [Command("addbn")]
        [Alias("addnominator", "createbn", "createnominator")]
        public async Task AddNominatorAsync(string mapId, string userId)
        {
            Map map = DB.GetFullMap(long.Parse(mapId));
            if (map == null)
            {
                await ReplyAsync("A beatmapset with that id doesn't exist");
                return;
            }

            Nominator nominator = DB.Get<Nominator>(long.Parse(userId));
            if (nominator == null)
            {
                nominator = OsuToDB.CreateNominator(long.Parse(userId));
                DB.Add(nominator);
            }

            map.Nominators.Add(nominator);
            DB.Update(map);

            await ReplyAsync("BN added to map");
        }


        [Command("rmbn")]
        [Alias("rmnominator", "removebn", "removenominator", "deletebn", "deletenominator")]
        public async Task RemoveNominatorAsync(string mapId, string userId)
        {
            Map map = DB.GetFullMap(long.Parse(mapId));
            if (map == null)
            {
                await ReplyAsync("A beatmapset with that id doesn't exist");
                return;
            }

            Nominator nominator = DB.Get<Nominator>(long.Parse(userId));
            if (nominator == null)
            {
                await ReplyAsync("A BN with that id doesn't exist");
                return;
            }

            map.Nominators.Remove(nominator);
            DB.Update(map);

            await ReplyAsync("BN removed from map");
        }




        [Command("changestatus")]
        [Alias("cs", "updatestatus", "changedescription", "updatedescription")]
        public async Task UpdateMapStatusAsync(string mapId, [Remainder] string status)
        {
            Map map = DB.GetFullMap(long.Parse(mapId));
            if (map == null)
            {
                await ReplyAsync("A beatmapset with that id doesn't exist");
                return;
            }

            map.Status = status;
            DB.Update(map);

            await ReplyAsync("Map status updated");
        }




        [Command("list")]
        [Alias("l", "all", "maps")]
        public async Task ListAsync()
        {
            List<Map> maps = DB.GetFullMaps();

            string reply = "";
            foreach (Map map in maps)
                reply += ShowMap(map) + "\n";

            await ReplyAsync(reply);
        }


        [Command("show")]
        [Alias("s", "map", "display")]
        public async Task ShowAsync(string mapId)
        {
            Map map = DB.GetFullMap(long.Parse(mapId));
            await ReplyAsync(ShowMap(map));
        }


        private string ShowMap(Map map)
        {
            string reply = "";
            reply +=
                    $"{ map.Artist } - { map.Name } ({ map.Category })\n" +
                    $"\tMapper: { map.Mapper.Name }\n";

            if (map.Status != null)
                reply += $"\tStatus: { map.Status }\n";

            if (map.Nominators.Count != 0)
            {
                reply += "\tBNs:\n";
                foreach (Nominator n in map.Nominators)
                    reply += $"\t\t{ n.Name }\n";
            }
            return reply;
        }




        [Command("info")]
        [Alias("i", "help", "h", "commands", "c")]
        public async Task InfoAsync()
        {
            await ReplyAsync(
@"```
<> = required
() = optional
|  = or
----------
Add a map and describe in what stage of the ranking/mapping proces it is.
!add   <map link|beatmapsetid>   (status)

Remove a previously added map.
!rm/rmmap   <map link|beatmapsetid|map title>

Assign a BN to a map. A map can have multiple BNs.
!addbn/addnominator   <map link|beatmapsetid|map title>   <bn link|userid|bn name>
```");
        }
    }
}
