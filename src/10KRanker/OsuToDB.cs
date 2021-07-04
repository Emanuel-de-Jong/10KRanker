using Database;
using OsuAPI;
using OsuSharp;
using System;
using System.Collections.Generic;

namespace _10KRanker
{
    public static class OsuToDB
    {
        public static Map ParseMap(Beatmap osuMap)
        {
            DateTime? osuUpdateDate = osuMap.LastUpdate.HasValue ? osuMap.LastUpdate.Value.DateTime : null;
            DateTime? osuAprovedDate = osuMap.ApprovedDate.HasValue ? osuMap.ApprovedDate.Value.DateTime : null;

            Map map = new(osuMap.BeatmapsetId, osuMap.Title, osuMap.Artist, (Database.Category)osuMap.State,
                osuMap.SubmitDate.Value.DateTime, osuUpdateDate, osuAprovedDate,
                DateTime.Now, DateTime.Now);

            map.Nominators = new List<Nominator>();
            return map;
        }

        public static Mapper ParseMapper(User osuUser)
        {
            Mapper mapper = new(osuUser.UserId, osuUser.Username);
            mapper.Maps = new List<Map>();
            return mapper;
        }

        public static Nominator ParseNominator(User osuUser)
        {
            Nominator nominator = new(osuUser.UserId, osuUser.Username);
            nominator.Maps = new List<Map>();
            return nominator;
        }


        public static Map CreateMap(long mapId, string status=null)
        {
            Beatmap osuMap = Osu.GetMap(mapId);

            Mapper dbMapper = DB.Get<Mapper>(osuMap.AuthorId);
            if (dbMapper == null)
                dbMapper = CreateMapper(osuMap.AuthorId);

            Map dbMap = ParseMap(osuMap);
            dbMap.Mapper = dbMapper;
            dbMap.Status = status;

            return dbMap;
        }

        public static Mapper CreateMapper(long mapperId)
        {
            return ParseMapper(Osu.GetUser(mapperId));
        }

        public static Mapper CreateMapper(string mapperName)
        {
            return ParseMapper(Osu.GetUser(mapperName));
        }

        public static Nominator CreateNominator(long nominatorId)
        {
            return ParseNominator(Osu.GetUser(nominatorId));
        }

        public static Nominator CreateNominator(string nominatorName)
        {
            return ParseNominator(Osu.GetUser(nominatorName));
        }
    }
}
