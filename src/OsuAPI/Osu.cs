using Logger;
using OsuSharp;
using System;

namespace OsuAPI
{
    public static class Osu
    {
        private static Log log;
        private static OsuClient client;
        public static OsuClient Client { get; }

        public static void Init()
        {
            log = new Log("osu");

            client = new(new OsuSharpConfiguration()
            {
                ApiKey = Secrets.ApiKey
            });
        }

        public static Beatmap GetMap(long beatmapsetId)
        {
            string logMessage = $"GetMap({ beatmapsetId });";

            var maps = client.GetBeatmapsetAsync(beatmapsetId).Result;

            if (maps.Count == 0)
            {
                log.Write(logMessage, "not found");
                throw new ArgumentException("A map with that id does not exist.");
            }

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
            {
                log.Write(logMessage, "not valid");
                throw new ArgumentException("The map doesn't have a 10K difficulty.");
            }

            if (map.State == BeatmapState.Ranked)
            {
                log.Write(logMessage, "not valid");
                throw new ArgumentException("The map is already ranked.");
            }

            if (map.DownloadUnavailable || map.AudioUnavailable)
            {
                log.Write(logMessage, "not valid");
                throw new ArgumentException("The map is corrupted.");
            }

            log.Write(logMessage);

            return map;
        }

        public static User GetUser(long userId)
        {
            string logMessage = $"GetUser({ userId });";

            User user = client.GetUserByUserIdAsync(userId, GameMode.Mania).Result;
            if (user == null)
            {
                log.Write(logMessage, "not found");
                throw new ArgumentException("A user with that id doesn't exist.");
            }

            log.Write(logMessage);

            return user;
        }

        public static User GetUser(string userName)
        {
            string logMessage = $"GetUser(\"{ userName }\");";

            User user = client.GetUserByUsernameAsync(userName, GameMode.Mania).Result;
            if (user == null)
            {
                log.Write(logMessage, "not found");
                throw new ArgumentException("A user with that name doesn't exist.");
            }

            log.Write(logMessage);

            return user;
        }
    }
}
