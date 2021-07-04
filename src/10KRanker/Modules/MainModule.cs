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


        private Map MapAliasToMap(string mapAlias)
        {
            Map map = null;
            InputType mapAliasType = Validator.GetMapInputType(mapAlias);
            switch (mapAliasType)
            {
                case InputType.Link:
                    map = DB.Get<Map>(Validator.MapLinkToId(mapAlias));
                    break;
                case InputType.Id:
                    map = DB.Get<Map>(Validator.StringToId(mapAlias));
                    break;
                case InputType.Name:
                    map = DB.GetMap(mapAlias);
                    break;
            }

            if (map == null)
                throw new ArgumentException("The map doesn't exist");

            return map;
        }




        [Command("add")]
        [Alias("addmap", "create", "createmap")]
        public async Task AddMapAsync(string mapAlias, [Remainder] string status = null)
        {
            long mapId = 0;
            bool mapExists = false;
            InputType mapAliasType = Validator.GetMapInputType(mapAlias);
            switch (mapAliasType)
            {
                case InputType.Link:
                    mapId = Validator.MapLinkToId(mapAlias);
                    mapExists = DB.Exists<Map>(mapId);
                    break;
                case InputType.Id:
                    mapId = Validator.StringToId(mapAlias);
                    mapExists = DB.Exists<Map>(mapId);
                    break;
                case InputType.Name:
                    mapExists = DB.MapExists(mapAlias);
                    break;
            }

            if (mapExists)
                throw new ArgumentException("The map already exists");

            if (mapAliasType == InputType.Name)
                throw new ArgumentException("Map names can only be used for existing maps. Try the map link or beatmapsetid instead");

            DB.Add(OsuToDB.CreateMap(mapId));

            await ReplyAsync("Map added");
        }


        [Command("rm")]
        [Alias("rmmap", "remove", "removemap", "delete", "deletemap")]
        public async Task RemoveMapAsync([Remainder] string mapAlias)
        {
            Map map = MapAliasToMap(mapAlias);

            DB.Remove(map);

            await ReplyAsync("Map removed");
        }




        [Command("addbn")]
        [Alias("addnominator", "createbn", "createnominator")]
        public async Task AddNominatorAsync(string mapAlias, string userAlias)
        {
            Map map = MapAliasToMap(mapAlias);

            long nominatorId = 0;
            Nominator nominator = null;
            InputType userAliasType = Validator.GetUserInputType(userAlias);
            switch (userAliasType)
            {
                case InputType.Link:
                    nominatorId = Validator.UserLinkToId(userAlias);
                    nominator = DB.Get<Nominator>(nominatorId);
                    break;
                case InputType.Id:
                    nominatorId = Validator.StringToId(userAlias);
                    nominator = DB.Get<Nominator>(nominatorId);
                    break;
                case InputType.Name:
                    nominator = DB.GetNominator(userAlias);
                    break;
            }

            if (nominator == null)
            {
                if (userAliasType == InputType.Name)
                {
                    nominator = OsuToDB.CreateNominator(userAlias);
                }
                else
                {
                    nominator = OsuToDB.CreateNominator(nominatorId);
                }
                DB.Add(nominator);
            }

            if (map.Nominators.Contains(nominator))
                throw new ArgumentException("The BN is already assigned to this map");

            map.Nominators.Add(nominator);
            DB.Save();

            await ReplyAsync("BN added to map");
        }


        [Command("rmbn")]
        [Alias("rmnominator", "removebn", "removenominator", "deletebn", "deletenominator")]
        public async Task RemoveNominatorAsync(string mapAlias, string userAlias)
        {
            Map map = MapAliasToMap(mapAlias);

            Nominator nominator = null;
            InputType userAliasType = Validator.GetUserInputType(userAlias);
            switch (userAliasType)
            {
                case InputType.Link:
                    nominator = DB.Get<Nominator>(Validator.UserLinkToId(userAlias));
                    break;
                case InputType.Id:
                    nominator = DB.Get<Nominator>(Validator.StringToId(userAlias));
                    break;
                case InputType.Name:
                    nominator = DB.GetNominator(userAlias);
                    break;
            }

            if (nominator == null)
                throw new ArgumentException("The BN doesn't exist");

            map.Nominators.Remove(nominator);
            DB.Save();

            await ReplyAsync("BN removed from map");
        }




        [Command("changestatus")]
        [Alias("cs", "updatestatus", "changedescription", "updatedescription")]
        public async Task UpdateMapStatusAsync(string mapAlias, [Remainder] string status)
        {
            Map map = MapAliasToMap(mapAlias);
            map.Status = status;
            DB.Save();

            await ReplyAsync("Map status updated");
        }




        [Command("show")]
        [Alias("s", "map", "display")]
        public async Task ShowAsync(string type, [Remainder] string mapAlias)
        {
            if (type == "map")
            {
                Map map = MapAliasToMap(mapAlias);
                await ReplyAsync(ShowMap(map));
            }
            else if (type == "mapper")
            {

            }
            else if (type == "bn")
            {

            }
        }


        [Command("list")]
        [Alias("l", "all", "maps")]
        public async Task ListAsync()
        {
            List<Map> maps = DB.GetMaps();

            string reply = "";
            foreach (Map map in maps)
                reply += ShowMap(map) + "\n";

            await ReplyAsync(reply);
        }




        [Command("info")]
        [Alias("i", "help", "h", "commands", "c")]
        public async Task InfoAsync()
        {
            await ReplyAsync(
@"
<> = required
() = optional
|  = or
"""" = read name with spaces as 1 value
----------
Add a map and describe in what stage of the ranking/mapping proces it is.
!add   <map link|beatmapsetid>   (status)

Remove a map.
!rm   <map link|beatmapsetid|map title>

Assign a BN to a map. A map can have multiple BNs.
!addbn   <map link|beatmapsetid|map title>   <bn link|userid|bn name>
");
        }
    }
}
