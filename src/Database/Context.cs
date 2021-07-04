using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Context : DbContext
    {
        public string[] TableNames { get; set; } = new string[] { "Maps", "Mappers", "Nominators", "MapNominator" };
        public DbSet<Map> Maps { get; set; }
        public DbSet<Mapper> Mappers { get; set; }
        public DbSet<Nominator> Nominators { get; set; }

        public void Init()
        {
            Database.ExecuteSqlRaw(
@"
SELECT * FROM Maps m
INNER JOIN Mappers mr ON m.MapperId = mr.MapperId
");

            Database.ExecuteSqlRaw(
@"
SELECT * FROM Maps m
INNER JOIN MapNominator mn ON m.MapId = mn.MapsMapId
INNER JOIN Nominators n ON mn.NominatorsNominatorId = n.NominatorId
");

            //Maps
            //    .Include(i => i.Mapper)
            //    .Include(i => i.Nominators)
            //    .DefaultIfEmpty();

            //Nominators
            //    .Include(i => i.Maps)
            //    .DefaultIfEmpty();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=D:\Coding\Repos\10KRanker\src\Database\10KRanked.db");
    }
}
