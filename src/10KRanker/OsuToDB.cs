using Database;
using OsuAPI;
using OsuSharp;
using System;

namespace _10KRanker
{
    public static class OsuToDB
    {
        public static Map ParseMap(Beatmap osuMap)
        {
            DateTime? osuUpdateDate = osuMap.LastUpdate.HasValue ? osuMap.LastUpdate.Value.DateTime : null;
            DateTime? osuAprovedDate = osuMap.ApprovedDate.HasValue ? osuMap.ApprovedDate.Value.DateTime : null;

            return new Map(osuMap.BeatmapsetId, osuMap.Title, osuMap.Artist, (Database.Category)osuMap.State,
                osuMap.SubmitDate.Value.DateTime, osuUpdateDate, osuAprovedDate,
                DateTime.Now, DateTime.Now);
        }

        public static Mapper ParseMapper(User osuUser)
        {
            return new Mapper(osuUser.UserId, osuUser.Username);
        }

        public static Nominator ParseNominator(User osuUser)
        {
            return new Nominator(osuUser.UserId, osuUser.Username);
        }


        public static Map CreateMap(long mapId)
        {
            return ParseMap(Osu.GetMap(mapId));
        }

        public static Map CreateFullMap(long mapId)
        {
            Beatmap osuMap = Osu.GetMap(mapId);

            Map dbMap = ParseMap(osuMap);

            Mapper dbMapper = DB.Get<Mapper>(osuMap.AuthorId);
            if (dbMapper == null)
                dbMapper = CreateMapper(osuMap.AuthorId);

            dbMap.Mapper = dbMapper;

            return dbMap;
        }

        public static Mapper CreateMapper(long userId)
        {
            return ParseMapper(Osu.GetUser(userId));
        }

        public static Nominator CreateNominator(long userId)
        {
            return ParseNominator(Osu.GetUser(userId));
        }
    }
}
