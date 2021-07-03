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

        public static T Get<T>(int objId) where T : class
        {
            return context.Find<T>(new int[] { objId });
        }

        public static List<T> Get<T>(List<int> objIds) where T : class
        {
            List<T> objs = new List<T>();
            foreach (int objId in objIds)
            {
                objs.Add(Get<T>(objId));
            }
            return objs;
        }

        public static List<Database.Map> GetMaps() => context.Maps.ToList();

        public static List<Database.Mapper> GetMappers() => context.Mappers.ToList();

        public static List<Database.Nominator> GetNominators() => context.Nominators.ToList();

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

        public static void Remove<T>(int objId) where T : class
        {
            Remove(Get<T>(objId));
        }

        public static void Remove(object obj)
        {
            context.Remove(obj);
            context.SaveChanges();
        }

        public static void Remove<T>(List<int> objIds) where T : class
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
