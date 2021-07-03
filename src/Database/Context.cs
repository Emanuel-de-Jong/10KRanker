﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Context : DbContext
    {
        public DbSet<Nominator> Nominators { get; set; }
        public DbSet<Mapper> Mappers { get; set; }
        public DbSet<Map> Maps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=D:\Coding\Repos\10KRanker\src\Database\10KRanked.db");
    }
}
