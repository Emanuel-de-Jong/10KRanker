using Database;
using Discord;
using Discord.Commands;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _10KRanker.Modules
{
    [RequireContext(ContextType.Guild)]
    public class MainModule : ModuleBase<SocketCommandContext>
    {
        private readonly Log log = ModuleHelper.Log;
        public CommandService CommandService { get; set; }


        [Command("a-unittest")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task UnitTestAsync(bool badWeather = true)
        {
            if (true) // ADD MAP
            {
                await AddMapAsync("1509186");
                await AddMapAsync("https://osu.ppy.sh/beatmapsets/1466367#mania/3011461");
                await AddMapAsync("1343787", "map3 state");
                await AddMapAsync("1095022");
                await AddMapAsync("1509353", "map5 %$#@test");

                if (badWeather) // BAD WEATHER
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

                if (badWeather) // BAD WEATHER
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

                if (badWeather) // BAD WEATHER
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

                if (badWeather) // BAD WEATHER
                {
                    await RemoveNominatorAsync("lalabai call me", "2225008");
                }
            }


            if (true) // UPDATE MAP STATUS
            {
                await UpdateMapStatusAsync("1466367", "map2 state");
                await UpdateMapStatusAsync("1343787", "map3 state edit1");
                await UpdateMapStatusAsync("1343787", "map3 state edit2");
            }


            await ListAsync();
        }


        [Command("a-cleardatabase")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ClearDatabaseAsync()
        {
            DB.ClearDatabase();

            await ReplyAsync("Database cleared.");
        }




        [Name("Show commands")]
        [Command("help")]
        [Alias("h", "info", "i", "commands", "c")]
        [Remarks("add")]
        [Summary("Show all commands. Or show more details about a specific command.")]
        public async Task InfoAsync(
            [Summary("(command)")] string commandAlias = null)
        {
            string message = $"InfoAsync(";
            if (commandAlias != null)
                message += $"\"{ commandAlias }\"";
            message += ");";

            try
            {
                if (commandAlias != null)
                {
                    var result = CommandService.Search(Context, commandAlias);

                    if (!result.IsSuccess)
                        throw new ArgumentException("There is no command with that name.");

                    CommandInfo command = result.Commands[0].Command;

                    await ReplyAsync(ModuleHelper.CommandToStringDetailed(command));
                }
                else
                {
                    List<CommandInfo> commands = CommandService.Commands.ToList();

                    string reply = "> **== Commands ==**\n" +
                        "> `<>` = Required     `\"\"` = Name/Title with spaces as 1 value\n" +
                        "> `()` = Optional       `|`  = Or\n";


                    foreach (CommandInfo command in commands)
                        if (command.Preconditions.Count == 0)
                            reply += "> \n" + ModuleHelper.CommandToString(command) + "\n";

                    await ReplyAsync(reply);
                }

                log.Write(message, ModuleHelper.SocketUserToString(Context.User));
            }
            catch (ArgumentException ae)
            {
                await ReplyAsync(ae.Message);
                log.Write(message, $"{ModuleHelper.SocketUserToString(Context.User)} - { ae.Message }");
            }
        }




        private const int MAPS_PER_MESSAGE = 10;
        [Name("List maps")]
        [Command("list")]
        [Alias("l", "all", "maps")]
        [Remarks("\"Komirin\"")]
        [Summary("List all maps. Optionally you can filter by mapper or beatmap nominator.")]
        public async Task ListAsync(
            [Summary("(user link|userid|user name)")][Remainder] string userAlias = null)
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

                    await ShowMaps(maps, "== **Maps** ==\n");
                }
                else if ((mapper = ModuleHelper.MapperAliasToMapper(userAlias, false)) != null)
                {
                    if (mapper.Maps.Count == 0)
                        throw new ArgumentException("The mapper doesn't have any maps in the bot's system.");

                    await ShowMaps(mapper.Maps, $"== **{mapper.Name}'s Maps** ==\n");
                }
                else if ((nominator = ModuleHelper.NominatorAliasToNominator(userAlias, false)) != null)
                {
                    if (nominator.Maps.Count == 0)
                        throw new ArgumentException("The BN isn't linked to any maps in the bot's system.");

                    await ShowMaps(nominator.Maps, $"== **Maps Linked to {nominator.Name}** ==\n");
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

        private async Task ShowMaps(List<Map> maps, string reply)
        {
            OsuToDB.UpdateMaps(maps);

            int mapCount = 0;
            foreach (Map map in maps)
            {
                reply += ModuleHelper.MapToString(map);

                mapCount++;
                if (mapCount == MAPS_PER_MESSAGE)
                {
                    mapCount = 0;
                    await ReplyAsync(reply);
                    reply = "‏‏‎ ‎\n";
                }
            }

            if (reply != "‏‏‎ ‎\n")
                await ReplyAsync(reply);
        }


        [Name("Show map")]
        [Command("show")]
        [Alias("s", "map", "display")]
        [Remarks("\"Last Resort\"")]
        [Summary("Show a map in full detail.")]
        public async Task ShowAsync(
            [Summary("<map link|beatmapsetid|map title>")][Remainder] string mapAlias)
        {
            try
            {
                Map map = ModuleHelper.MapAliasToMap(mapAlias);

                OsuToDB.UpdateMap(map);

                await ReplyAsync(ModuleHelper.MapToStringDetailed(map));
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




        [Name("Add map")]
        [Command("add")]
        [Alias("addmap", "create", "createmap")]
        [Remarks("1234567   \"Almost ready for modding.\"")]
        [Summary("Add a map to the bot. Optionally you can give a short description about the current mapping state of the map.")]
        public async Task AddMapAsync(
            [Summary("<map link|beatmapsetid|map title>")] string mapAlias,
            [Summary("(mapping state)")] [Remainder] string status = null)
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


        [Name("Remove map")]
        [Command("rm")]
        [Alias("rmmap", "remove", "removemap", "delete", "deletemap")]
        [Remarks("\"Last Resort\"")]
        [Summary("Remove a map from the bot.")]
        public async Task RemoveMapAsync(
            [Summary("<map link|beatmapsetid|map title>")] [Remainder] string mapAlias)
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




        [Name("Update mapping state")]
        [Command("updatestate")]
        [Alias("updatestatus", "updateprogress", "changestate", "changestatus", "changeprogress")]
        [Remarks("\"Last Resort\"   \"Open for modding now.Any help is appreciated!\"")]
        [Summary("Update the mapping state of a map. This is the same state as the optional value of `!add`.")]
        public async Task UpdateMapStatusAsync(
            [Summary("<map link|beatmapsetid|map title>")] string mapAlias,
            [Summary("<mapping state>")] [Remainder] string status)
        {
            try
            {
                Map map = ModuleHelper.MapAliasToMap(mapAlias);
                map.Status = status;
                DB.Update(map);

                await ReplyAsync("The mapping state has been changed.");
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




        [Name("Add BN to map")]
        [Command("addbn")]
        [Alias("addnominator", "createbn", "createnominator")]
        [Remarks("\"Last Resort\"   \"Komirin\"")]
        [Summary("Link a beatmap nominator to a map. This shows that the BN has helped/will help with the modding/ranking of the map. A map can have multiple BNs.")]
        public async Task AddNominatorAsync(
            [Summary("<map link|beatmapsetid|map title>")] string mapAlias,
            [Summary("<bn link|userid|bn name>")] [Remainder] string nominatorAlias)
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


        [Name("Remove BN from map")]
        [Command("rmbn")]
        [Alias("rmnominator", "removebn", "removenominator", "deletebn", "deletenominator")]
        [Remarks("\"Last Resort\"   \"Komirin\"")]
        [Summary("Remove a beatmap nominator from a map.")]
        public async Task RemoveNominatorAsync(
            [Summary("<map link|beatmapsetid|map title>")] string mapAlias,
            [Summary("<bn link|userid|bn name>")] [Remainder] string nominatorAlias)
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
    }
}
