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
                osuMap = Osu.GetMap(1193846); // 1193846 good | 1393811 std | 1369976 7k
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            User osuUser = null;
            try
            {
                osuUser = Osu.GetUser(10948555); // 10948555
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
