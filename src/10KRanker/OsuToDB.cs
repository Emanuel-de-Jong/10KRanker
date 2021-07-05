using Database;
using OsuAPI;
using OsuSharp;
using System;
using System.Collections.Generic;
using System.Timers;

namespace _10KRanker
{
    public static class OsuToDB
    {
        private static DBTable lastOnUpdateTable = DBTable.Map;

        public static void OnUpdateDBTablesTimerElapsed(object source, ElapsedEventArgs e)
        {
            lastOnUpdateTable = (DBTable)(((int)lastOnUpdateTable + 1) % 3);
            Console.WriteLine("Sheduled update of " + lastOnUpdateTable);

            if (lastOnUpdateTable == DBTable.Map)
            {
                UpdateMaps(DB.GetMaps());
            }
            else if (lastOnUpdateTable == DBTable.Mapper)
            {
                UpdateMappers(DB.GetMappers());
            }
            else if (lastOnUpdateTable == DBTable.Nominator)
            {
                UpdateNominators(DB.GetNominators());
            }
        }


        public static void UpdateMap(Map dbMap)
        {
            if (dbMap.LastUpdateCheck > DateTime.Now.AddHours(-3))
                return;
            dbMap.LastUpdateCheck = DateTime.Now;

            Beatmap osuMap = null;
            try
            {
                osuMap = Osu.GetMap(dbMap.MapId);
            }
            catch (ArgumentException ae)
            {
                DB.Remove(dbMap);
                return;
            }

            bool changed = false;

            if (dbMap.Name != osuMap.Title)
            {
                changed = true;
                dbMap.Name = osuMap.Title;
            }
            if (dbMap.Artist != osuMap.Artist)
            {
                changed = true;
                dbMap.Artist = osuMap.Artist;
            }
            if (dbMap.Category != (Category)osuMap.State)
            {
                changed = true;
                dbMap.Category = (Category)osuMap.State;
            }
            DateTime? osuUpdateDate = osuMap.LastUpdate.HasValue ? osuMap.LastUpdate.Value.DateTime : null;
            if (dbMap.OsuUpdateDate != osuUpdateDate)
            {
                changed = true;
                dbMap.OsuUpdateDate = osuUpdateDate;
            }
            DateTime? osuAprovedDate = osuMap.ApprovedDate.HasValue ? osuMap.ApprovedDate.Value.DateTime : null;
            if (dbMap.OsuAprovedDate != osuAprovedDate)
            {
                changed = true;
                dbMap.OsuAprovedDate = osuAprovedDate;
            }

            if (changed)
                DB.Update(dbMap);
        }

        public static void UpdateMaps(List<Map> dbMaps)
        {
            foreach (Map map in dbMaps)
                UpdateMap(map);
        }

        public static void UpdateMapper(Mapper dbMapper)
        {
            User osuUser = null;
            try
            {
                osuUser = Osu.GetUser(dbMapper.MapperId);
            }
            catch (ArgumentException ae)
            {
                DB.Remove(dbMapper);
                return;
            }

            if (dbMapper.Name != osuUser.Username)
            {
                dbMapper.Name = osuUser.Username;
                DB.Update(dbMapper);
            }
        }

        public static void UpdateMappers(List<Mapper> dbMappers)
        {
            foreach (Mapper mapper in dbMappers)
                UpdateMapper(mapper);
        }

        public static void UpdateNominator(Nominator dbNominator)
        {
            User osuUser = null;
            try
            {
                osuUser = Osu.GetUser(dbNominator.NominatorId);
            }
            catch (ArgumentException ae)
            {
                DB.Remove(dbNominator);
                return;
            }

            if (dbNominator.Name != osuUser.Username)
            {
                dbNominator.Name = osuUser.Username;
                DB.Update(dbNominator);
            }
        }

        public static void UpdateNominators(List<Nominator> dbNominators)
        {
            foreach (Nominator nominator in dbNominators)
                UpdateNominator(nominator);
        }

        public static Map ParseMap(Beatmap osuMap)
        {
            DateTime? osuUpdateDate = osuMap.LastUpdate.HasValue ? osuMap.LastUpdate.Value.DateTime : null;
            DateTime? osuAprovedDate = osuMap.ApprovedDate.HasValue ? osuMap.ApprovedDate.Value.DateTime : null;

            Map map = new(osuMap.BeatmapsetId, osuMap.Title, osuMap.Artist, (Category)osuMap.State,
                osuMap.SubmitDate.Value.DateTime, osuUpdateDate, osuAprovedDate,
                DateTime.Now, DateTime.Now);

            map.Nominators = new List<Nominator>();
            return map;
        }

        public static Mapper ParseMapper(User osuUser)
        {
            Mapper mapper = new(osuUser.UserId, osuUser.Username);
            mapper.Maps = new List<Map>();
            return mapper;
        }

        public static Nominator ParseNominator(User osuUser)
        {
            Nominator nominator = new(osuUser.UserId, osuUser.Username);
            nominator.Maps = new List<Map>();
            return nominator;
        }


        public static Map CreateMap(long mapId, string status=null)
        {
            Beatmap osuMap = Osu.GetMap(mapId);

            Mapper dbMapper = DB.Get<Mapper>(osuMap.AuthorId);
            if (dbMapper == null)
                dbMapper = CreateMapper(osuMap.AuthorId);

            Map dbMap = ParseMap(osuMap);
            dbMap.Mapper = dbMapper;
            dbMap.Status = status;

            return dbMap;
        }

        public static Mapper CreateMapper(long mapperId)
        {
            return ParseMapper(Osu.GetUser(mapperId));
        }

        public static Mapper CreateMapper(string mapperName)
        {
            return ParseMapper(Osu.GetUser(mapperName));
        }

        public static Nominator CreateNominator(long nominatorId)
        {
            return ParseNominator(Osu.GetUser(nominatorId));
        }

        public static Nominator CreateNominator(string nominatorName)
        {
            return ParseNominator(Osu.GetUser(nominatorName));
        }
    }
}
