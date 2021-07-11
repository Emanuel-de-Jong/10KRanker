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
        private static DBModel onUpdateTable = DBModel.Map;
        private static Timer updateDBTablesTimer;

        public static void Init()
        {
            updateDBTablesTimer = new Timer(1 * 24 * 60 * 60 * 1000);
            updateDBTablesTimer.AutoReset = true;
            updateDBTablesTimer.Elapsed += OnUpdateDBTablesTimerElapsed;
            updateDBTablesTimer.Start();

            OnUpdateDBTablesTimerElapsed(null, null);
        }

        public static void OnUpdateDBTablesTimerElapsed(object s, ElapsedEventArgs e)
        {
            Console.WriteLine("Sheduled update of " + onUpdateTable);

            if (onUpdateTable == DBModel.Map)
            {
                UpdateMaps(DB.GetMaps());
            }
            else if (onUpdateTable == DBModel.Mapper)
            {
                UpdateMappers(DB.GetMappers());
            }
            else if (onUpdateTable == DBModel.Nominator)
            {
                UpdateNominators(DB.GetNominators());
            }

            onUpdateTable = (DBModel)(((int)onUpdateTable + 1) % 3);
        }


        public static void UpdateMap(Map dbMap)
        {
            if (dbMap.LastUpdateCheck > DateTime.Now.AddHours(-3))
                return;

            dbMap.LastUpdateCheck = DateTime.Now;

            Beatmap osuMap;
            try
            {
                osuMap = Osu.GetMap(dbMap.MapId);
            }
            catch (ArgumentException)
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
            User osuUser;
            try
            {
                osuUser = Osu.GetUser(dbMapper.MapperId);
            }
            catch (ArgumentException)
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
            User osuUser;
            try
            {
                osuUser = Osu.GetUser(dbNominator.NominatorId);
            }
            catch (ArgumentException)
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


        public static Map CreateMap(long mapId, string status = null)
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
