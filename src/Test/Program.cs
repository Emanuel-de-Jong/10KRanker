using _10KRanker;
using Database;
using OsuAPI;
using OsuSharp;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DBTable table = DBTable.Map;

            for (int i = 0; i < 20; i++)
            {
                table = (DBTable)(((int)table + 1) % 3);
                Console.WriteLine(table);
            }
        }

        private static void DBTest()
        {
            var maps = DB.GetMaps();
            var mappers = DB.GetMappers();
            var nominators = DB.GetNominators();
            Console.WriteLine();
        }

        private static void ValidatorTest()
        {
            Validator.MapLinkToId("https://osu.ppy.sh/beatmapsets/1488095#mania/3050710");
            Validator.MapLinkToId("https://osu.ppy.sh/beatmapsets/1488095");
            Validator.UserLinkToId("https://osu.ppy.sh/users/10948555/mania");
            Validator.UserLinkToId("https://osu.ppy.sh/users/10948555");
        }

        private static void OsuTest()
        {
            Beatmap osuMap = null;
            try
            {
                osuMap = Osu.GetMap(1499988); // 1193846 BorW | 1343787 Lalabai | 1393811 std | 1369976 7k
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            PrintProperties(osuMap);


            //User osuUser = null;
            //try
            //{
            //    osuUser = Osu.GetUser(4815717); // 10948555 KBot | 4815717 Feerum
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //PrintProperties(osuUser);
        }

        private static void PrintProperties(object obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(obj, null));
            }
        }
    }
}
