using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

namespace Database
{
    public class DB
    {
        public static string dbName = "db.db";
        public static string dbPath = @"" + dbName;

        private Context context;

        public DB() => DBAsync();

        private async Task DBAsync()
        {
            await using var context = new Context();
            this.context = context;

            CRUD._Context = context;

            await init();

            foreach (var nominator in await Nominator.ReadAll())
            {
                Console.WriteLine(nominator.Name);
            }
        }

        private async Task init()
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                await Nominator.Create(new List<Nominator>()
                {
                    new Nominator("Test1"),
                    new Nominator("Test2"),
                });
            }
            catch (Exception)
            {
                File.Delete(dbPath);
                await init();
            }
        }
    }
}
