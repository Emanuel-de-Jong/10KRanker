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
            Beatmap osuMap = null;
            try
            {
                osuMap = Osu.GetMap(1343787); // 1193846 BorW | 1343787 Lalabai | 1393811 std | 1369976 7k
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            PrintProperties(osuMap);


            User osuUser = null;
            try
            {
                osuUser = Osu.GetUser(4815717); // 10948555 KBot | 4815717 Feerum
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            PrintProperties(osuUser);
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
