using OsuSharp;
using System;
using System.Threading.Tasks;

namespace OsuAPI
{
    public static class Osu
    {
        private static OsuClient client = new(new OsuSharpConfiguration() { ApiKey = Secrets.ApiKey });
        public static OsuClient Client { get; }

        public static Beatmap GetMap(long beatmapsetId)
        {
            var maps = client.GetBeatmapsetAsync(beatmapsetId).Result;
            if (maps.Count == 0)
                throw new ArgumentException("A beatmapset with that id does not exist");

            Beatmap map = null;
            foreach (Beatmap m in maps)
            {
                if (m.GameMode == GameMode.Mania && m.CircleSize == 10)
                {
                    map = m;
                    break;
                }
            }

            if (map == null)
                throw new ArgumentException("The beatmapset does not have a 10K difficulty");

            return map;
        }

        public static Beatmap GetMap(string beatmapsetId)
        {
            return GetMap(long.Parse(beatmapsetId));
        }

        public static User GetUser(long userId)
        {
            User user = client.GetUserByUserIdAsync(userId, GameMode.Mania).Result;
            if (user == null)
                throw new ArgumentException("A user with that id does not exist");

            return user;
        }

        public static User GetUser(string userId)
        {
            return GetUser(long.Parse(userId));
        }
    }
}
