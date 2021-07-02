using OsuSharp;
using System.Threading.Tasks;

namespace OsuAPI
{
    public class OsuAPIClient
    {
        private OsuClient client;

        public OsuAPIClient()
        {
            var config = new OsuSharpConfiguration();
            config.ApiKey = Secrets.ApiKey;

            client = new OsuClient(config);
        }

        public Beatmap GetBeatmap(int beatmapsetid)
        {
            return client.GetBeatmapByIdAsync(beatmapsetid).Result;
        }

        public User GetUser(int userid)
        {
            return client.GetUserByUserIdAsync(userid, GameMode.Mania).Result;
        }
    }
}
