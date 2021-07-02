using OsuSharp;
using System.Threading.Tasks;

namespace OsuAPI
{
    public static class Osu
    {
        private static OsuClient client = new(new OsuSharpConfiguration() { ApiKey = Secrets.ApiKey });
        public static OsuClient Client { get; }

        public static Beatmap GetBeatmap(int beatmapsetId)
        {
            return client.GetBeatmapByIdAsync(beatmapsetId).Result;
        }

        public static User GetUser(int userId)
        {
            return client.GetUserByUserIdAsync(userId, GameMode.Mania).Result;
        }
    }
}
