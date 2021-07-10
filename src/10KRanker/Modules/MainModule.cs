using Database;
using Discord.Commands;
using Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _10KRanker.Modules
{
    public class MainModule : ModuleBase<SocketCommandContext>
    {
        private Log log = ModuleHelper.Log;


        //[Command("unittest")]
        public async Task TestAsync()
        {
            if (true) // ADD MAP
            {
                await AddMapAsync("1509186");
                await AddMapAsync("https://osu.ppy.sh/beatmapsets/1466367#mania/3011461");
                await AddMapAsync("1343787", "map3 status");
                await AddMapAsync("1095022");
                await AddMapAsync("1509353", "map5 %$#@test");

                if (true) // BAD WEATHER
                {
                    await AddMapAsync("osu.ppy.sh/beatmapsets/11X3846");
                    await AddMapAsync("osu.ppy.sh/bla");
                    await AddMapAsync("abc123test321cba");
                    await AddMapAsync("99999999");
                    await AddMapAsync("1393811");
                    await AddMapAsync("1369976");
                    await AddMapAsync("1193846");
                    await AddMapAsync("1509186");
                    await AddMapAsync("10K Indo Pack #2");
                }
            }


            if (true) // REMOVE MAP
            {
                await RemoveMapAsync("1095022");

                if (true) // BAD WEATHER
                {
                    await RemoveMapAsync("1095022");
                }
            }


            if (true) // ADD NOMINATOR
            {
                await AddNominatorAsync("1509186", "4815717");
                await AddNominatorAsync("1509186", "Unpredictable");
                await AddNominatorAsync("1466367", "osu.ppy.sh/users/259972/fruits");
                await AddNominatorAsync("1343787", "2225008");

                if (true) // BAD WEATHER
                {
                    await AddNominatorAsync("1509186", "osu.ppy.sh/bla");
                    await AddNominatorAsync("1509186", "abc123test321cba");
                    await AddNominatorAsync("1509186", "99999999");
                    await AddNominatorAsync("1509186", "4815717");
                    await AddNominatorAsync("1509186", "feerum");
                }
            }


            if (true) // REMOVE NOMINATOR
            {
                await RemoveNominatorAsync("lalabai call me", "2225008");

                if (true) // BAD WEATHER
                {
                    await RemoveNominatorAsync("lalabai call me", "2225008");
                }
            }


            if (true) // UPDATE MAP STATUS
            {
                await UpdateMapStatusAsync("1466367", "map2 status");
                await UpdateMapStatusAsync("1343787", "map3 status edit1");
                await UpdateMapStatusAsync("1343787", "map3 status edit2");
            }


            await ListAsync();
        }




        [Command("add")]
        [Alias("addmap", "create", "createmap")]
        public async Task AddMapAsync(string mapAlias, [Remainder] string status = null)
        {
            string message = $"AddMapAsync(\"{ mapAlias }\"";
            if (status != null)
                message += $", \"{ status }\"";
            message += ");";

            try
            {
                long mapId = 0;
                Map map = ModuleHelper.MapAliasToMap(mapAlias, false, out mapId);

                if (map != null)
                    throw new ArgumentException("The map is already in the bot's system.");

                if (mapId == -1)
                    throw new ArgumentException("Map names can only be used for maps in the bot's system. Try the map link or beatmapsetid instead.");

                DB.Add(OsuToDB.CreateMap(mapId, status));

                await ReplyAsync("The map has been added.");
                log.Write(message, ModuleHelper.SocketUserToString(Context.User));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
                log.Write(message, $"{ModuleHelper.SocketUserToString(Context.User)} - { ae.Message }");
            }
        }


        [Command("rm")]
        [Alias("rmmap", "remove", "removemap", "delete", "deletemap")]
        public async Task RemoveMapAsync([Remainder] string mapAlias)
        {
            try
            {
                Map map = ModuleHelper.MapAliasToMap(mapAlias);

                DB.Remove(map);

                await ReplyAsync("The map has been removed.");
                log.Write($"RemoveMapAsync(\"{ mapAlias }\");",
                    ModuleHelper.SocketUserToString(Context.User));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
                log.Write($"RemoveMapAsync(\"{ mapAlias }\");",
                    $"{ModuleHelper.SocketUserToString(Context.User)} - { ae.Message }");
            }
        }




        [Command("changestatus")]
        [Alias("cs", "updatestatus", "changedescription", "updatedescription")]
        public async Task UpdateMapStatusAsync(string mapAlias, [Remainder] string status)
        {
            try
            {
                Map map = ModuleHelper.MapAliasToMap(mapAlias);
                map.Status = status;
                DB.Update(map);

                await ReplyAsync("The map status has been changed.");
                log.Write($"UpdateMapStatusAsync(\"{ mapAlias }\", \"{ status }\");",
                    ModuleHelper.SocketUserToString(Context.User));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
                log.Write($"UpdateMapStatusAsync(\"{ mapAlias }\", \"{ status }\");",
                    $"{ModuleHelper.SocketUserToString(Context.User)} - { ae.Message }");
            }
        }




        [Command("addbn")]
        [Alias("addnominator", "createbn", "createnominator")]
        public async Task AddNominatorAsync(string mapAlias, string nominatorAlias)
        {
            try
            {
                Map map = ModuleHelper.MapAliasToMap(mapAlias);

                long nominatorId = 0;
                Nominator nominator = ModuleHelper.NominatorAliasToNominator(nominatorAlias, false, out nominatorId);

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
                log.Write($"AddNominatorAsync(\"{ mapAlias }\", \"{ nominatorAlias }\");",
                    ModuleHelper.SocketUserToString(Context.User));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
                log.Write($"AddNominatorAsync(\"{ mapAlias }\", \"{ nominatorAlias }\");",
                    $"{ModuleHelper.SocketUserToString(Context.User)} - { ae.Message }");
            }
        }


        [Command("rmbn")]
        [Alias("rmnominator", "removebn", "removenominator", "deletebn", "deletenominator")]
        public async Task RemoveNominatorAsync(string mapAlias, string nominatorAlias)
        {
            try
            {
                Map map = ModuleHelper.MapAliasToMap(mapAlias);
                Nominator nominator = ModuleHelper.NominatorAliasToNominator(nominatorAlias);

                if (!map.Nominators.Contains(nominator))
                    throw new ArgumentException("The BN already wasn't linked to the map.");

                map.Nominators.Remove(nominator);
                DB.Update(map);

                await ReplyAsync("The BN has been removed from the map.");
                log.Write($"RemoveNominatorAsync(\"{ mapAlias }\", \"{ nominatorAlias }\");",
                    ModuleHelper.SocketUserToString(Context.User));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
                log.Write($"RemoveNominatorAsync(\"{ mapAlias }\", \"{ nominatorAlias }\");",
                    $"{ModuleHelper.SocketUserToString(Context.User)} - { ae.Message }");
            }
        }




        [Command("show")]
        [Alias("s", "map", "display")]
        public async Task ShowAsync([Remainder] string mapAlias)
        {
            try
            {
                Map map = ModuleHelper.MapAliasToMap(mapAlias);

                OsuToDB.UpdateMap(map);

                await ReplyAsync(ModuleHelper.MapToString(map));
                log.Write($"ShowAsync(\"{ mapAlias }\");",
                    ModuleHelper.SocketUserToString(Context.User));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
                log.Write($"ShowAsync(\"{ mapAlias }\");",
                    $"{ModuleHelper.SocketUserToString(Context.User)} - { ae.Message }");
            }
        }


        [Command("list")]
        [Alias("l", "all", "maps")]
        public async Task ListAsync([Remainder] string userAlias = null)
        {
            string message = $"ListAsync(";
            if (userAlias != null)
                message += $"\"{ userAlias }\"";
            message += ");";

            try
            {
                Mapper mapper;
                Nominator nominator;
                if (userAlias == null)
                {
                    List<Map> maps = DB.GetMaps();

                    if (maps.Count == 0)
                        throw new ArgumentException("There are no maps at the moment.");

                    OsuToDB.UpdateMaps(maps);

                    await ReplyAsync("== **Maps** ==\n" + ModuleHelper.MapsToString(maps));
                }
                else if ((mapper = ModuleHelper.MapperAliasToMapper(userAlias, false)) != null)
                {
                    if (mapper.Maps.Count == 0)
                        throw new ArgumentException("The mapper doesn't have any maps in the bot's system.");

                    OsuToDB.UpdateMaps(mapper.Maps);

                    await ReplyAsync($"== **{mapper.Name}'s Maps** ==\n" + ModuleHelper.MapsToString(mapper.Maps));
                }
                else if ((nominator = ModuleHelper.NominatorAliasToNominator(userAlias, false)) != null)
                {
                    if (nominator.Maps.Count == 0)
                        throw new ArgumentException("The BN isn't linked to any maps in the bot's system.");

                    OsuToDB.UpdateMaps(nominator.Maps);

                    await ReplyAsync($"== **Maps Linked to {nominator.Name}** ==\n" + ModuleHelper.MapsToString(nominator.Maps));
                }
                else
                {
                    throw new ArgumentException("The user is not in the bot's system.");
                }

                log.Write(message, ModuleHelper.SocketUserToString(Context.User));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
                log.Write(message, $"{ModuleHelper.SocketUserToString(Context.User)} - { ae.Message }");
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

            log.Write($"InfoAsync();",
                ModuleHelper.SocketUserToString(Context.User));
        }
    }
}
