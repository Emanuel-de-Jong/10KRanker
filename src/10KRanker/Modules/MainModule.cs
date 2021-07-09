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

        [Command("add")]
        [Alias("addmap", "create", "createmap")]
        public async Task AddMapAsync(string mapAlias, [Remainder] string status = null)
        {
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

                // Log command call
                string message = $"AddMapAsync(\"{ mapAlias }\"";
                if (status != null)
                    message += $", \"{ status }\"";
                message += ");";
                log.Write(message, ModuleHelper.SocketUserToString(Context.User));
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
                Map map = ModuleHelper.MapAliasToMap(mapAlias);

                DB.Remove(map);

                await ReplyAsync("The map has been removed.");

                // Log command call
                log.Write($"RemoveMapAsync(\"{ mapAlias }\");",
                    ModuleHelper.SocketUserToString(Context.User));
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
                Map map = ModuleHelper.MapAliasToMap(mapAlias);
                map.Status = status;
                DB.Update(map);

                await ReplyAsync("The map status has been changed.");

                // Log command call
                log.Write($"UpdateMapStatusAsync(\"{ mapAlias }\", \"{ status }\");",
                    ModuleHelper.SocketUserToString(Context.User));
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

                // Log command call
                log.Write($"AddNominatorAsync(\"{ mapAlias }\", \"{ nominatorAlias }\");",
                    ModuleHelper.SocketUserToString(Context.User));
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
                Map map = ModuleHelper.MapAliasToMap(mapAlias);
                Nominator nominator = ModuleHelper.NominatorAliasToNominator(nominatorAlias);

                if (!map.Nominators.Contains(nominator))
                    throw new ArgumentException("The BN already wasn't linked to the map.");

                map.Nominators.Remove(nominator);
                DB.Update(map);

                await ReplyAsync("The BN has been removed from the map.");

                // Log command call
                log.Write($"RemoveNominatorAsync(\"{ mapAlias }\", \"{ nominatorAlias }\");",
                    ModuleHelper.SocketUserToString(Context.User));
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
                Map map = ModuleHelper.MapAliasToMap(mapAlias);

                OsuToDB.UpdateMap(map);

                await ReplyAsync(ModuleHelper.MapToString(map));

                // Log command call
                log.Write($"ShowAsync(\"{ mapAlias }\");",
                    ModuleHelper.SocketUserToString(Context.User));
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


                // Log command call
                string message = $"ListAsync(";
                if (userAlias != null)
                    message += $"\"{ userAlias }\"";
                message += ");";
                log.Write(message, ModuleHelper.SocketUserToString(Context.User));
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

            // Log command call
            log.Write($"InfoAsync();",
                ModuleHelper.SocketUserToString(Context.User));
        }
    }
}
