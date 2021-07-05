using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
    public static class DB
    {
        private static Context context = new Context().Init();
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
                context.Database.ExecuteSqlRaw($"DELETE FROM { table };");
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

        public static bool MapExists(string mapName)
        {
            return GetMap(mapName) != null;
        }

        public static bool MapperExists(string mapperName)
        {
            return GetMapper(mapperName) != null;
        }

        public static bool NominatorExists(string nominatorName)
        {
            return GetNominator(nominatorName) != null;
        }


        public static T Get<T>(long objId) where T : class
        {
            return context.Find<T>(new object[] { objId });
        }

        public static Map GetMap(string mapName)
        {
            foreach (Map map in context.Maps)
                if (map.Name.ToLower() == mapName.ToLower())
                    return map;
            return null;
        }

        public static Mapper GetMapper(string mapperName)
        {
            foreach (Mapper mapper in context.Mappers)
                if (mapper.Name.ToLower() == mapperName.ToLower())
                    return mapper;
            return null;
        }

        public static Nominator GetNominator(string nominatorName)
        {
            foreach (Nominator nominator in context.Nominators)
                if (nominator.Name.ToLower() == nominatorName.ToLower())
                    return nominator;
            return null;
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

        public static List<Map> GetMaps() => context.Maps.ToList();

        public static List<Mapper> GetMappers() => context.Mappers.ToList();

        public static List<Nominator> GetNominators() => context.Nominators.ToList();


        public static void Update(object obj)
        {
            if (obj is Map)
            {
                Map map = obj as Map;
                map.UpdateDate = DateTime.Now;
            }

            context.SaveChanges();
        }

        public static void Update(List<object> objs)
        {
            if (objs.Count > 0 && objs[0] is Map)
            {
                foreach (Map map in objs)
                {
                    map.UpdateDate = DateTime.Now;
                }
            }

            context.SaveChanges();
        }


        public static void Remove(object obj)
        {
            context.Remove(obj);
            context.SaveChanges();
        }

        public static void Remove<T>(long objId) where T : class
        {
            Remove(Get<T>(objId));
        }

        public static void RemoveMap(string mapName)
        {
            Remove(GetMap(mapName));
        }

        public static void RemoveMapper(string mapperName)
        {
            Remove(GetMapper(mapperName));
        }

        public static void RemoveNominator(string nominatorName)
        {
            Remove(GetNominator(nominatorName));
        }

        public static void Remove(List<object> objs)
        {
            context.RemoveRange(objs);
            context.SaveChanges();
        }

        public static void Remove<T>(List<long> objIds) where T : class
        {
            Remove(Get<T>(objIds));
        }
    }
}
