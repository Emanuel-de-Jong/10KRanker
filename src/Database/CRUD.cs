using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class CRUD<T> where T : class
    {
        public Context _Context { get; set; }
        public DbSet<T> DBSet { get; set; }

        public CRUD(DbSet<T> dbSet, Context context)
        {
            this.DBSet = dbSet;
            this._Context = context;
        }


        public async Task Create(T obj)
        {
            DBSet.Add(obj);
            await _Context.SaveChangesAsync();
        }

        public async Task Create(List<T> objs)
        {
            await DBSet.AddRangeAsync(objs);
            await _Context.SaveChangesAsync();
        }

        public Task<List<T>> ReadAll()
        {
            return Task.FromResult(DBSet.ToList());
        }
    }
}
