using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Context : DbContext
    {
        public static string DBPath { get; set; }

        public string[] TableNames { get; set; } = new string[] { "Maps", "Mappers", "Nominators", "MapNominator" };
        public DbSet<Map> Maps { get; set; }
        public DbSet<Mapper> Mappers { get; set; }
        public DbSet<Nominator> Nominators { get; set; }

        public Context Init()
        {
            Maps
                .Include(i => i.Mapper)
                .Include(i => i.Nominators)
                .DefaultIfEmpty().ToList();

            return this;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            DBPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\10KRanked.db";
            options.UseSqlite(@"Data Source=" + DBPath);
        }



        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<Map>()
        //        .HasIndex(i => i.Name);

        //    builder.Entity<Mapper>()
        //        .HasIndex(i => i.Name)
        //        .IsUnique();

        //    builder.Entity<Nominator>()
        //        .HasIndex(i => i.Name)
        //        .IsUnique();
        //}
    }
}
