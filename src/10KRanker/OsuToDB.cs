using Database.Models;
using OsuAPI;
using OsuSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10KRanker
{
    public static class OsuToDB
    {
        public static Map ParseMap(Beatmap osuMap)
        {
            return new Map(osuMap.BeatmapsetId, osuMap.Title, osuMap.Artist, (Database.Enums.Category)osuMap.State,
                DateTime.Now, DateTime.Now, osuMap.LastUpdate.Value.DateTime);
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
