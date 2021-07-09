using _10KRanker;
using Database;
using Logger;
using OsuAPI;
using OsuSharp;
using System;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\10KRanked\10KRanked";

            var maps1 = DB.GetMaps();
            var map = maps1[0];
            Console.WriteLine($"{map.Name} - {map.Status}");

            Console.ReadKey();
            File.Copy(path + ".db", path + "2.db", true);

            map.Status = map.Status + "!";
            DB.Update(map);
            
            var maps2 = DB.GetMaps();
            Console.WriteLine($"{maps2[0].Name} - {maps2[0].Status}");

            Console.ReadKey();
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
