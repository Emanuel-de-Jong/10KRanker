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
