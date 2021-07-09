﻿using Database;
using Discord.WebSocket;
using Logger;
using System;
using System.Collections.Generic;

namespace _10KRanker.Modules
{
    public static class ModuleHelper
    {
        public static Log Log { get; } = new Log("commands");


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




        public static string MapsToString(List<Map> maps)
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

        public static string MapToString(Map m)
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
