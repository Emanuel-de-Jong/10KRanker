using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public abstract class CRUD
    {
        public static Context _Context { get; set; }
        public static DbSet<Nominator> DBSet { get; set; }

        public static async Task Create(Nominator obj)
        {
            DBSet.Add(obj);
            await _Context.SaveChangesAsync();
        }

        public static async Task Create(List<Nominator> objs)
        {
            await DBSet.AddRangeAsync(objs);
            await _Context.SaveChangesAsync();
        }

        public static Task<List<Nominator>> ReadAll()
        {
            return Task.FromResult(DBSet.ToList());
        }
    }
}
