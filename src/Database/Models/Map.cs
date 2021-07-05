using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    public enum Category
    {
        Graveyard = -2,
        WIP = -1,
        Pending = 0,
        Ranked = 1,
        Approved = 2,
        Qualified = 3,
        Loved = 4
    }


    [Index(nameof(Name))]
    public class Map
    {
        public long MapId { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Status { get; set; }
        public Category Category { get; set; }
        public DateTime OsuSubmitDate { get; set; }
        public DateTime? OsuUpdateDate { get; set; }
        public DateTime? OsuAprovedDate { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public long MapperId { get; set; }
        public Mapper Mapper { get; set; }

        public List<Nominator> Nominators { get; set; }

        [NotMapped]
        public DateTime LastUpdateCheck { get; set; } = DateTime.MinValue;

        public Map(long mapId, string name, string artist, Category category,
            DateTime osuSubmitDate, DateTime? osuUpdateDate, DateTime? osuAprovedDate,
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
