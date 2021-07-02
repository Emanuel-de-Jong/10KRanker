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
            return client.GetBeatmapByIdAsync(beatmapsetId).Result;
        }

        public static Beatmap GetMap(string beatmapsetId)
        {
            return GetMap(Int32.Parse(beatmapsetId));
        }

        public static User GetUser(long userId)
        {
            return client.GetUserByUserIdAsync(userId, GameMode.Mania).Result;
        }

        public static User GetUser(string userId)
        {
            return GetUser(Int32.Parse(userId));
        }
    }
}
