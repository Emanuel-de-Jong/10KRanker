﻿using System;
using System.Collections.Generic;

namespace Database
{
    public class Map
    {
        public long MapId { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Status { get; set; }
        public Category Category { get; set; }
        public DateTime OsuSubmitDate { get; set; }
        public DateTime OsuUpdateDate { get; set; }
        public DateTime OsuAprovedDate { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public int MapperId { get; set; }
        public Mapper Mapper { get; set; }

        public List<Nominator> Nominators { get; set; }

        public Map(long mapId, string name, string artist, Category category,
            DateTime osuSubmitDate, DateTime osuUpdateDate, DateTime osuAprovedDate,
            DateTime submitDate, DateTime updateDate)
        {
            this.MapId = mapId;
            this.Name = name;
            this.Artist = artist;
            this.Category = category;
            this.OsuSubmitDate = osuSubmitDate;
            this.OsuUpdateDate = osuUpdateDate;
            this.OsuAprovedDate = osuAprovedDate;
            this.SubmitDate = submitDate;
            this.UpdateDate = updateDate;
        }
    }
}