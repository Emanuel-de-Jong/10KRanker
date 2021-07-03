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
            var osuMap = Osu.GetMap(mapId);

            var dbMap = OsuToDB.ParseMap(osuMap);
            var dbMapper = OsuToDB.CreateMapper(osuMap.AuthorId);

            dbMap.Mapper = dbMapper;

            DB.Add(dbMap);

            await ReplyAsync("Map saved");
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
    }
}
