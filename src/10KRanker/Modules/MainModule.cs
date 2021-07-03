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
        public async Task AddAsync(string mapId)
        {
            if (DB.Exists<Map>(long.Parse(mapId)))
            {
                await ReplyAsync("A beatmapset with that id already exists");
                return;
            }

            DB.Add(OsuToDB.CreateFullMap(long.Parse(mapId)));

            await ReplyAsync("Map saved");
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
                nominator = OsuToDB.CreateNominator(long.Parse(userId));

            map.Nominators.Add(nominator);

            DB.Update(map);

            await ReplyAsync("BN added to map");
        }

        [Command("list")]
        [Alias("l", "all")]
        public async Task ListAsync()
        {
            List<Map> maps = DB.GetMaps();

            string reply = "";
            foreach (Map m in maps)
            {
                reply += $"{m.Artist} - {m.Name}\n\t{m.Mapper.Name}\n";
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
