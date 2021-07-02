using Database.Models;
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
        public static Mapper ParseMapper(User osuUser)
        {
            return new Mapper(osuUser.UserId, osuUser.Username);
        }

        public static Map ParseMap(Beatmap osuMap)
        {
            return new Map(osuMap.BeatmapId, osuMap.Title + osuMap.Difficulty, "",
                (Database.Enums.Category)osuMap.State, DateTime.Now, DateTime.Now, DateTime.Now);
        }
    }
}
