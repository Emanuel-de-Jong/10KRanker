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
        private string MapIdToLink(long mapId)
        {
            return $"<https://osu.ppy.sh/beatmapsets/{ mapId }>";
        }

        private string UserIdToLink(long userId)
        {
            return $"<https://osu.ppy.sh/users/{ userId }>";
        }

        private string MapsToString(List<Map> maps)
        {
            string reply = "";
            foreach (Map m in maps)
            {
                reply += $"**{ m.Artist } - { m.Name }** ({ MapIdToLink(m.MapId) })\n";
                if (m.OsuUpdateDate == null)
                {
                    reply += $"\tSubmitted to osu: { m.OsuSubmitDate }\n";
                }
                else
                {
                    reply += $"\tLast updated on osu: { m.OsuUpdateDate }\n";
                }
                reply += $"\tCategory: { m.Category }\tMapper: { m.Mapper.Name }";

                if (m.Nominators.Count != 0)
                    reply += $"\tBN(s): { String.Join(", ", m.Nominators) }";

                reply += "\n";

                if (m.Status != null)
                    reply += $"\tStatus: { m.Status }\n";

                reply += "\n";
            }
            return reply;
        }

        private string MapToString(Map m)
        {
            string reply = $"**{ m.Artist } - { m.Name }** ({ MapIdToLink(m.MapId) })\n";
            if (m.OsuUpdateDate == null)
            {
                reply += $"Submitted to osu: { m.OsuSubmitDate }\n";
            }
            else
            {
                reply += $"Last updated on osu: { m.OsuUpdateDate }\n";
            }

            if (m.OsuAprovedDate != null)
                reply += $"Aproved on osu: { m.OsuAprovedDate }\n";

            reply += $"Submmitted here: { m.SubmitDate }\n" +
            $"Last updated here: { m.UpdateDate }\n" +
            $"Mapper: { m.Mapper.Name } ({ UserIdToLink(m.MapperId) })\n" +
            $"Category: { m.Category }\n" +
            "Status: ";
            if (m.Status != null)
            {
                reply += m.Status + "\n";
            }
            else
            {
                reply += "None\n";
            }

            reply += $"BN(s):";
            if (m.Nominators.Count == 0)
            {
                reply += " None\n";
            }
            else
            {
                reply += "\n";
                foreach (Nominator n in m.Nominators)
                    reply += $"\t{ n.Name } ({ UserIdToLink(n.NominatorId) })\n";
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

        private Mapper MapperAliasToMapper(string mapperAlias)
        {
            Mapper mapper = null;
            InputType userAliasType = Validator.GetUserInputType(mapperAlias);
            switch (userAliasType)
            {
                case InputType.Link:
                    mapper = DB.Get<Mapper>(Validator.UserLinkToId(mapperAlias));
                    break;
                case InputType.Id:
                    mapper = DB.Get<Mapper>(Validator.StringToId(mapperAlias));
                    break;
                case InputType.Name:
                    mapper = DB.GetMapper(mapperAlias);
                    break;
            }

            if (mapper == null)
                throw new ArgumentException("The mapper doesn't exist");

            return mapper;
        }

        private Nominator NominatorAliasToNominator(string nominatorAlias)
        {
            Nominator nominator = null;
            InputType userAliasType = Validator.GetUserInputType(nominatorAlias);
            switch (userAliasType)
            {
                case InputType.Link:
                    nominator = DB.Get<Nominator>(Validator.UserLinkToId(nominatorAlias));
                    break;
                case InputType.Id:
                    nominator = DB.Get<Nominator>(Validator.StringToId(nominatorAlias));
                    break;
                case InputType.Name:
                    nominator = DB.GetNominator(nominatorAlias);
                    break;
            }

            if (nominator == null)
                throw new ArgumentException("The BN doesn't exist");

            return nominator;
        }




        [Command("add")]
        [Alias("addmap", "create", "createmap")]
        public async Task AddMapAsync(string mapAlias, [Remainder] string status = null)
        {
            try
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
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
            }
        }


        [Command("rm")]
        [Alias("rmmap", "remove", "removemap", "delete", "deletemap")]
        public async Task RemoveMapAsync([Remainder] string mapAlias)
        {
            try
            {
                Map map = MapAliasToMap(mapAlias);

                DB.Remove(map);

                await ReplyAsync("Map removed");
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
            }
        }




        [Command("addbn")]
        [Alias("addnominator", "createbn", "createnominator")]
        public async Task AddNominatorAsync(string mapAlias, string nominatorAlias)
        {
            try
            {
                Map map = MapAliasToMap(mapAlias);

                long nominatorId = 0;
                Nominator nominator = null;
                InputType userAliasType = Validator.GetUserInputType(nominatorAlias);
                switch (userAliasType)
                {
                    case InputType.Link:
                        nominatorId = Validator.UserLinkToId(nominatorAlias);
                        nominator = DB.Get<Nominator>(nominatorId);
                        break;
                    case InputType.Id:
                        nominatorId = Validator.StringToId(nominatorAlias);
                        nominator = DB.Get<Nominator>(nominatorId);
                        break;
                    case InputType.Name:
                        nominator = DB.GetNominator(nominatorAlias);
                        break;
                }

                if (nominator == null)
                {
                    if (userAliasType == InputType.Name)
                    {
                        nominator = OsuToDB.CreateNominator(nominatorAlias);
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
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
            }
        }


        [Command("rmbn")]
        [Alias("rmnominator", "removebn", "removenominator", "deletebn", "deletenominator")]
        public async Task RemoveNominatorAsync(string mapAlias, string nominatorAlias)
        {
            try
            {
                Map map = MapAliasToMap(mapAlias);
                Nominator nominator = NominatorAliasToNominator(nominatorAlias);

                if (!map.Nominators.Contains(nominator))
                    throw new ArgumentException("The BN is not linked to the map already");

                map.Nominators.Remove(nominator);
                DB.Save();

                await ReplyAsync("BN removed from map");
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
            }
        }




        [Command("changestatus")]
        [Alias("cs", "updatestatus", "changedescription", "updatedescription")]
        public async Task UpdateMapStatusAsync(string mapAlias, [Remainder] string status)
        {
            try
            {
                Map map = MapAliasToMap(mapAlias);
                map.Status = status;
                DB.Save();

                await ReplyAsync("Map status updated");
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
            }
        }




        [Command("show")]
        [Alias("s", "map", "display")]
        public async Task ShowAsync(string type, [Remainder] string typeAlias)
        {
            try
            {
                if (type == "map")
                {
                    Map map = MapAliasToMap(typeAlias);
                    await ReplyAsync(MapToString(map));
                }
                else if (type == "mapper")
                {
                    Mapper mapper = MapperAliasToMapper(typeAlias);

                    if (mapper.Maps.Count == 0)
                        throw new ArgumentException("The mapper doesn't have any maps");

                    await ReplyAsync(MapsToString(mapper.Maps));
                }
                else if (type == "bn")
                {
                    Nominator nominator = NominatorAliasToNominator(typeAlias);

                    if (nominator.Maps.Count == 0)
                        throw new ArgumentException("The BN isn't linked to any maps");

                    await ReplyAsync(MapsToString(nominator.Maps));
                }
                else
                {
                    throw new ArgumentException("The type format is wrong. Try map, mapper or bn instead.");
                }
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
            }
        }


        [Command("list")]
        [Alias("l", "all", "maps")]
        public async Task ListAsync()
        {
            try
            {
                List<Map> maps = DB.GetMaps();

                if (maps.Count == 0)
                    throw new ArgumentException("There are no maps at the moment");

                await ReplyAsync(MapsToString(maps));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
            }
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
"""" = read name with spaces as 1 value
----------
Add a map and describe in what stage of the ranking/mapping proces it is.
!add   <map link|beatmapsetid>   (status)

Remove a map.
!rm   <map link|beatmapsetid|map title>

Assign a BN to a map. A map can have multiple BNs.
!addbn   <map link|beatmapsetid|map title>   <bn link|userid|bn name>
```");
        }
    }
}
