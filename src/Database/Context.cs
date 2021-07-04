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

        public Context() : base()
        {
            Maps
                .Include(i => i.Mapper)
                .Include(i => i.Nominators)
                .DefaultIfEmpty().ToList();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=E:\Coding\Repos\10KRanker\src\Database\10KRanked.db");



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
