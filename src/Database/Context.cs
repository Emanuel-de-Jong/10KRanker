using GlobalValues;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace Database
{
    public class Context : DbContext
    {
        public static string DBDirPath { get; } = G.AssetPath;
        public static string DBPath { get; } = DBDirPath + $"{G.DS}10KRanker.db";

        public string[] TableNames { get; } = new string[] { "Maps", "Mappers", "Nominators", "MapNominator" };
        public DbSet<Map> Maps { get; set; }
        public DbSet<Mapper> Mappers { get; set; }
        public DbSet<Nominator> Nominators { get; set; }


        public void Init()
        {
            _ = Maps
                .Include(i => i.Mapper)
                .Include(i => i.Nominators)
                .DefaultIfEmpty().ToList();

            Backup.Init();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!Directory.Exists(DBDirPath))
                Directory.CreateDirectory(DBDirPath);

            options.UseSqlite(@"Data Source=" + DBPath);
        }
    }
}
