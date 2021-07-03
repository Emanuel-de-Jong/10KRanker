using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
    public static class DB
    {
        private static Context context = new();
        public static Context Context { get; } = context;


        public static void Save()
        {
            context.SaveChanges();
        }

        public static void Dispose()
        {
            context.Dispose();
        }


        public static void ClearDatabase()
        {
            foreach (string table in context.TableNames)
                context.Database.ExecuteSqlRawAsync($"DELETE FROM { table };");
            context.SaveChanges();
        }


        public static void Add(object obj)
        {
            context.Add(obj);
            context.SaveChanges();
        }

        public static void Add(List<object> objs)
        {
            context.AddRange(objs);
            context.SaveChanges();
        }


        public static bool Exists<T>(long objId) where T : class
        {
            return Get<T>(objId) != null;
        }

        public static T Get<T>(long objId) where T : class
        {
            return context.Find<T>(new object[] { objId });
        }

        public static List<T> Get<T>(List<long> objIds) where T : class
        {
            List<T> objs = new List<T>();
            foreach (long objId in objIds)
            {
                objs.Add(Get<T>(objId));
            }
            return objs;
        }

        public static Map GetFullMap(long mapId)
        {
            return context.Maps
                .Include(i => i.Mapper)
                .Include(i => i.Nominators)
                .FirstOrDefault(x => x.MapId == mapId);
        }

        public static Mapper GetFullMapper(long mapperId)
        {
            return Get<Mapper>(mapperId);
        }

        public static Nominator GetFullNominator(long nominatorId)
        {
            return context.Nominators
                .Include(i => i.Maps)
                .FirstOrDefault(x => x.NominatorId == nominatorId);
        }

        public static List<Map> GetMaps() => context.Maps.ToList();

        public static List<Mapper> GetMappers() => context.Mappers.ToList();

        public static List<Nominator> GetNominators() => context.Nominators.ToList();

        public static List<Map> GetFullMaps()
        {
            return context.Maps
                .Include(i => i.Mapper)
                .Include(i => i.Nominators)
                .DefaultIfEmpty().ToList();
        }

        public static List<Mapper> GetFullMappers() => context.Mappers.ToList();

        public static List<Nominator> GetFullNominators() => context.Nominators.ToList();


        public static void Update(object obj)
        {
            context.Update(obj);
            context.SaveChanges();
        }

        public static void Update(List<object> objs)
        {
            context.UpdateRange(objs);
            context.SaveChanges();
        }


        public static void Remove<T>(long objId) where T : class
        {
            Remove(Get<T>(objId));
        }

        public static void Remove(object obj)
        {
            context.Remove(obj);
            context.SaveChanges();
        }

        public static void Remove<T>(List<long> objIds) where T : class
        {
            Remove(Get<T>(objIds));
        }

        public static void Remove(List<object> objs)
        {
            context.RemoveRange(objs);
            context.SaveChanges();
        }
    }
}
