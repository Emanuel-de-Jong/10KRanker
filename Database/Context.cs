using Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Database
{
    public class Context : DbContext
    {
        public static DbSet<Nominator> Nominators { get; set; }

        //public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=db", option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nominator>().ToTable("Nominators", "test");
            modelBuilder.Entity<Nominator>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasIndex(i => i.Name).IsUnique();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
