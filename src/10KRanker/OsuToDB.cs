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
            return new Map(osuMap.BeatmapsetId, osuMap.Title, osuMap.Artist, (Database.Category)osuMap.State,
                osuMap.SubmitDate.Value.DateTime, osuMap.LastUpdate.Value.DateTime, osuMap.ApprovedDate.Value.DateTime,
                DateTime.Now, DateTime.Now);
        }

        public static Mapper ParseMapper(User osuUser)
        {
            return new Mapper(osuUser.UserId, osuUser.Username);
        }

        public static Map CreateMap(long mapId)
        {
            return ParseMap(Osu.GetMap(mapId));
        }

        public static Map CreateMap(string mapId)
        {
            return ParseMap(Osu.GetMap(mapId));
        }

        public static Mapper CreateMapper(long userId)
        {
            return ParseMapper(Osu.GetUser(userId));
        }

        public static Mapper CreateMapper(string userId)
        {
            return ParseMapper(Osu.GetUser(userId));
        }
    }
}
