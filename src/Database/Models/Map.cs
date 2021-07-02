using System;
using System.Collections.Generic;

namespace Database.Models
{
    public class Map
    {
        public int MapId { get; set; }
        public int BeatmapsetId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime OsuModificationDate { get; set; }

        public int MapperId { get; set; }
        public Mapper Mapper { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Nominator> Nominators { get; set; }

        public Map(int beatmapsetId, string name, string status,
            string creationDate, string modificationDate, string osuModificationDate)
        {
            this.BeatmapsetId = beatmapsetId;
            this.Name = name;
            this.Status = status;
            this.CreationDate = DateTime.Parse(creationDate);
            this.ModificationDate = DateTime.Parse(modificationDate);
            this.OsuModificationDate = DateTime.Parse(osuModificationDate);
        }

        public Map(int beatmapsetId, string name, string status,
            DateTime creationDate, DateTime modificationDate, DateTime osuModificationDate)
        {
            this.BeatmapsetId = beatmapsetId;
            this.Name = name;
            this.Status = status;
            this.CreationDate = creationDate;
            this.ModificationDate = modificationDate;
            this.OsuModificationDate = osuModificationDate;
        }
    }
}
