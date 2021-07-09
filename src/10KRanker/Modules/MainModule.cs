using Database;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _10KRanker.Modules
{
    public class MainModule : ModuleBase<SocketCommandContext>
    {
        [Command("add")]
        [Alias("addmap", "create", "createmap")]
        public async Task AddMapAsync(string mapAlias, [Remainder] string status = null)
        {
            try
            {
                long mapId = 0;
                Map map = MapAliasToMap(mapAlias, false, out mapId);

                if (map != null)
                    throw new ArgumentException("The map is already in the bot's system.");

                if (mapId == -1)
                    throw new ArgumentException("Map names can only be used for maps in the bot's system. Try the map link or beatmapsetid instead.");

                DB.Add(OsuToDB.CreateMap(mapId, status));

                await ReplyAsync("The map has been added.");
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

                await ReplyAsync("The map has been removed.");
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
                DB.Update(map);

                await ReplyAsync("The map status has been changed.");
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
                Nominator nominator = NominatorAliasToNominator(nominatorAlias, false, out nominatorId);

                if (nominator == null)
                {
                    if (nominatorId == -1)
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
                    throw new ArgumentException("The BN is already linked to the map.");

                map.Nominators.Add(nominator);
                DB.Update(map);

                await ReplyAsync("The BN has been linked to the map.");
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
                    throw new ArgumentException("The BN already wasn't linked to the map.");

                map.Nominators.Remove(nominator);
                DB.Update(map);

                await ReplyAsync("The BN has been removed from the map.");
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
            }
        }




        [Command("show")]
        [Alias("s", "map", "display")]
        public async Task ShowAsync([Remainder] string mapAlias)
        {
            try
            {
                Map map = MapAliasToMap(mapAlias);

                OsuToDB.UpdateMap(map);

                await ReplyAsync(MapToString(map));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
            }
        }


        [Command("list")]
        [Alias("l", "all", "maps")]
        public async Task ListAsync([Remainder] string userAlias = null)
        {
            try
            {
                if (userAlias == null)
                {
                    List<Map> maps = DB.GetMaps();

                    if (maps.Count == 0)
                        throw new ArgumentException("There are no maps at the moment.");

                    OsuToDB.UpdateMaps(maps);

                    await ReplyAsync("== **Maps** ==\n" + MapsToString(maps));
                    return;
                }

                Mapper mapper;
                Nominator nominator;
                if ((mapper = MapperAliasToMapper(userAlias, false)) != null)
                {
                    if (mapper.Maps.Count == 0)
                        throw new ArgumentException("The mapper doesn't have any maps in the bot's system.");

                    OsuToDB.UpdateMaps(mapper.Maps);

                    await ReplyAsync($"== **{mapper.Name}'s Maps** ==\n" + MapsToString(mapper.Maps));
                }
                else if ((nominator = NominatorAliasToNominator(userAlias, false)) != null)
                {
                    if (nominator.Maps.Count == 0)
                        throw new ArgumentException("The BN isn't linked to any maps in the bot's system.");

                    OsuToDB.UpdateMaps(nominator.Maps);

                    await ReplyAsync($"== **Maps Linked to {nominator.Name}** ==\n" + MapsToString(nominator.Maps));
                }
                else
                {
                    throw new ArgumentException("The user is not in the bot's system.");
                }
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
@"
== **Commands** ==
`<>` = Required
`()` = Optional
`|`   = Or
`""""` = Send a name/title with spaces as 1 value

**Maps**
> `!add`   `<map link|beatmapsetid>`   `(status)`
Add a map and describe in what stage of the mapping/ranking/modding proces it is.
> `!rm`   `<map link|beatmapsetid|map title>`
Remove a map.
> `!changestatus`   `<map link|beatmapsetid|map title>`   `<status>`
Change the status of a map. The status describes how the mapping/ranking/modding of a map is going.

**Beatmap Nominators**
> `!addbn`   `<map link|beatmapsetid|map title>`   `<bn link|userid|bn name>`
Link a BN to a map. A map can have multiple BNs.
> `!rmbn`   `<map link|beatmapsetid|map title>`   `<bn link|userid|bn name>`
Remove a BN from a map.

**Show maps**
> `!show`   `<map link|beatmapsetid|map title>`
Show the detials of a map.
> `!list`   `(user link|userid|user name)`
List all maps, the maps of a mapper or the maps linked to a BN.

**Other**
> `!info`
Show this message.
----------
");
        }




        private static string MapIdToLink(long mapId)
        {
            return $"<https://osu.ppy.sh/beatmapsets/{ mapId }>";
        }


        private static string UserIdToLink(long userId)
        {
            return $"<https://osu.ppy.sh/users/{ userId }>";
        }




        private static string MapsToString(List<Map> maps)
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
                    reply += $"\tBN(s): { string.Join(", ", m.Nominators) }";

                reply += "\n";

                if (m.Status != null)
                    reply += $"\tStatus: { m.Status }\n";

                reply += "\n";
            }
            return reply;
        }

        private static string MapToString(Map m)
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


        private static Map MapAliasToMap(string mapAlias, bool throwIfNotExists = true)
            => MapAliasToMap(mapAlias, throwIfNotExists, out long _);
        private static Map MapAliasToMap(string mapAlias, bool throwIfNotExists, out long mapId)
        {
            mapId = -1;
            Map map = null;
            InputType mapAliasType = Validator.GetMapInputType(mapAlias);
            switch (mapAliasType)
            {
                case InputType.Link:
                    mapId = Validator.MapLinkToId(mapAlias);
                    map = DB.Get<Map>(mapId);
                    break;
                case InputType.Id:
                    mapId = Validator.StringToId(mapAlias);
                    map = DB.Get<Map>(mapId);
                    break;
                case InputType.Name:
                    map = DB.GetMap(mapAlias);
                    break;
            }

            if (throwIfNotExists && map == null)
                throw new ArgumentException("The map is not in the bot's system.");

            return map;
        }


        private static Mapper MapperAliasToMapper(string mapperAlias, bool throwIfNotExists = true)
            => MapperAliasToMapper(mapperAlias, throwIfNotExists, out long _);
        private static Mapper MapperAliasToMapper(string mapperAlias, bool throwIfNotExists, out long mapperId)
        {
            mapperId = -1;
            Mapper mapper = null;
            InputType userAliasType = Validator.GetUserInputType(mapperAlias);
            switch (userAliasType)
            {
                case InputType.Link:
                    mapperId = Validator.UserLinkToId(mapperAlias);
                    mapper = DB.Get<Mapper>(mapperId);
                    break;
                case InputType.Id:
                    mapperId = Validator.StringToId(mapperAlias);
                    mapper = DB.Get<Mapper>(mapperId);
                    break;
                case InputType.Name:
                    mapper = DB.GetMapper(mapperAlias);
                    break;
            }

            if (throwIfNotExists && mapper == null)
                throw new ArgumentException("The mapper is not in the bot's system.");

            return mapper;
        }


        private static Nominator NominatorAliasToNominator(string nominatorAlias, bool throwIfNotExists = true)
            => NominatorAliasToNominator(nominatorAlias, throwIfNotExists, out long _);
        private static Nominator NominatorAliasToNominator(string nominatorAlias, bool throwIfNotExists, out long nominatorId)
        {
            nominatorId = -1;
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

            if (throwIfNotExists && nominator == null)
                throw new ArgumentException("The BN is not in the bot's system.");

            return nominator;
        }
    }
}
