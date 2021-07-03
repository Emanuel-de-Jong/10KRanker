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
        [Alias("addmap")]
        public async Task AddMapAsync(string mapId, [Remainder] string status)
        {
            if (DB.Exists<Map>(long.Parse(mapId)))
            {
                await ReplyAsync("A beatmapset with that id already exists");
                return;
            }

            Map map = OsuToDB.CreateFullMap(long.Parse(mapId));
            map.Status = status;

            DB.Add(map);

            await ReplyAsync("Map added");
        }


        [Command("rm")]
        [Alias("rmmap")]
        public async Task RemoveMapAsync(string mapId)
        {
            Map map = DB.Get<Map>(long.Parse(mapId));
            if (map == null)
            {
                await ReplyAsync("A beatmapset with that id doesn't exist");
                return;
            }

            DB.Remove(map);

            await ReplyAsync("Map removed");
        }




        [Command("addbn")]
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
        [Alias("cs")]
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
        [Alias("l", "all")]
        public async Task ListAsync()
        {
            List<Map> maps = DB.GetFullMaps();

            string reply = "";
            foreach (Map m in maps)
            {
                reply +=
                    $"{ m.Artist } - { m.Name } ({ m.Category })\n" +
                    $"\tStatus: { m.Status }\n" +
                    $"\tMapper: { m.Mapper.Name }\n";

                if (m.Nominators.Count != 0)
                {
                    reply += "\tBNs:\n";
                    foreach (Nominator n in m.Nominators)
                        reply += $"\t\t{ n.Name }\n";
                }

                reply += "\n";
            }

            await ReplyAsync(reply);
        }


        [Command("show")]
        public async Task ShowAsync(string mapId)
        {
            Map map = DB.GetFullMap(long.Parse(mapId));

            await ReplyAsync($"{map.Artist} - {map.Name}\n\t{map.Mapper.Name}");
        }
    }
}
