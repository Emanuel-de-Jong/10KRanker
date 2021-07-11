using Database;
using Discord.Commands;
using Discord.WebSocket;
using Logger;
using System;
using System.Collections.Generic;

namespace _10KRanker.Modules
{
    public static class ModuleHelper
    {
        public static Log Log { get; } = new Log("commands");
        public static string DateFormat { get; } = "M/d/yyy hh:mm tt";


        public static string SocketUserToString(SocketUser user)
        {
            return user.ToString();
        }

        public static string MapIdToLink(long mapId)
        {
            return $"<https://osu.ppy.sh/beatmapsets/{ mapId }>";
        }


        public static string UserIdToLink(long userId)
        {
            return $"<https://osu.ppy.sh/users/{ userId }>";
        }




        public static string MapToString(Map m)
        {
            string reply = "";
            reply += $"**{ m.Artist } - { m.Name }** ({ MapIdToLink(m.MapId) })\n";
            if (m.OsuUpdateDate == null)
            {
                reply += $"\tSubmitted to osu: { m.OsuSubmitDate.ToString(DateFormat) }\n";
            }
            else
            {
                reply += $"\tLast updated on osu: { m.OsuUpdateDate.Value.ToString(DateFormat) }\n";
            }
            reply += $"\tCategory: { m.Category }\tMapper: { m.Mapper.Name }";

            if (m.Nominators.Count != 0)
            {
                reply += "\tBN(s): ";

                bool firstLoop = true;
                foreach (Nominator n in m.Nominators)
                {
                    if (firstLoop)
                    {
                        firstLoop = false;
                        reply += n.Name;
                        continue;
                    }

                    reply += $", {n.Name}";
                }
            }

            reply += "\n";

            if (m.Status != null)
                reply += $"\tStatus: { m.Status }\n";

            reply += "\n";
            return reply;
        }


        public static string MapToStringDetailed(Map m)
        {
            string reply = $"**{ m.Artist } - { m.Name }** ({ MapIdToLink(m.MapId) })\n";
            if (m.OsuUpdateDate == null)
            {
                reply += $"Submitted to osu: { m.OsuSubmitDate.ToString(DateFormat) }\n";
            }
            else
            {
                reply += $"Last updated on osu: { m.OsuUpdateDate.Value.ToString(DateFormat) }\n";
            }

            if (m.OsuAprovedDate != null)
                reply += $"Aproved on osu: { m.OsuAprovedDate.Value.ToString(DateFormat) }\n";

            reply += $"Submmitted here: { m.SubmitDate.ToString(DateFormat) }\n" +
            $"Last updated here: { m.UpdateDate.ToString(DateFormat) }\n" +
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


        public static string CommandToString(CommandInfo command)
        {
            string cmd = "!" + command.Aliases[0];

            string parameters = "";
            foreach (var parameter in command.Parameters)
            {
                parameters += $"   { parameter.Summary }";
            }

            return $"> **{ command.Name }**\n" +
                $"> `{ cmd }{ parameters }`\n" +
                $"> `{ cmd }   {command.Remarks}`";
        }

        public static string CommandToStringDetailed(CommandInfo command)
        {
            string cmd = "!" + command.Aliases[0];
            string alias = $"`!{ string.Join("`  `!", command.Aliases) }`";

            string parameters = "";
            foreach (var parameter in command.Parameters)
            {
                parameters += $"   { parameter.Summary }";
            }

            return $"> **{ command.Name }**\n" +
                $"> Description:     { command.Summary }\n" +
                $"> Aliases:            { alias }\n" +
                $"> Syntax:             `{ cmd }{ parameters }`\n" +
                $"> Example:          `{ cmd }   {command.Remarks}`";
        }


        public static Map MapAliasToMap(string mapAlias, bool throwIfNotExists = true)
            => MapAliasToMap(mapAlias, throwIfNotExists, out long _);
        public static Map MapAliasToMap(string mapAlias, bool throwIfNotExists, out long mapId)
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


        public static Mapper MapperAliasToMapper(string mapperAlias, bool throwIfNotExists = true)
            => MapperAliasToMapper(mapperAlias, throwIfNotExists, out long _);
        public static Mapper MapperAliasToMapper(string mapperAlias, bool throwIfNotExists, out long mapperId)
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


        public static Nominator NominatorAliasToNominator(string nominatorAlias, bool throwIfNotExists = true)
            => NominatorAliasToNominator(nominatorAlias, throwIfNotExists, out long _);
        public static Nominator NominatorAliasToNominator(string nominatorAlias, bool throwIfNotExists, out long nominatorId)
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
