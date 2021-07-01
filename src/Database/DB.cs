using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Database.Models;

namespace Database
{
    public class DB
    {
        public static string dbName = "db.db";
        public static string dbPath = @"" + dbName;

        private Context context;

        private CRUD<Nominator> nominatorCRUD;
        private CRUD<Mapper> mapperCRUD;

        public DB() => DBAsync();

        private async Task DBAsync()
        {
            await using var context = new Context();
            this.context = context;

            //CRUD._Context = context;
            nominatorCRUD = new CRUD<Nominator>(context.Nominators, context);
            mapperCRUD = new CRUD<Mapper>(context.Mappers, context);


            await Init();


            await nominatorCRUD.Create(new List<Nominator>()
                {
                    new Nominator("Nominator 1"),
                    new Nominator("Nominator 2"),
                });

            foreach (var nominator in await nominatorCRUD.ReadAll())
            {
                Console.WriteLine(nominator.Name);
            }


            await mapperCRUD.Create(new List<Mapper>()
                {
                    new Mapper("Mapper 1"),
                    new Mapper("Mapper 2"),
                });

            foreach (var mapper in await mapperCRUD.ReadAll())
            {
                Console.WriteLine(mapper.Name);
            }
        }

        private async Task Init()
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
            }
            catch (Exception)
            {
                File.Delete(dbPath);
                await Init();
            }
        }
    }
}
