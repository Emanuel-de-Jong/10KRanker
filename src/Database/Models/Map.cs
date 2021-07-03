using System;
using System.Collections.Generic;

namespace Database
{
    public class Map
    {
        public long MapId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Artist { get; set; }
        public Category Category { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime OsuModificationDate { get; set; }

        public int MapperId { get; set; }
        public Mapper Mapper { get; set; }

        public List<Nominator> Nominators { get; set; }

        public Map(long mapId, string name, string artist, Category category,
            string creationDate, string modificationDate, string osuModificationDate)
        {
            this.MapId = mapId;
            this.Name = name;
            this.Artist = artist;
            this.Category = category;
            this.CreationDate = DateTime.Parse(creationDate);
            this.ModificationDate = DateTime.Parse(modificationDate);
            this.OsuModificationDate = DateTime.Parse(osuModificationDate);
        }

        public Map(long mapId, string name, string artist, Category category,
            DateTime creationDate, DateTime modificationDate, DateTime osuModificationDate)
        {
            this.MapId = mapId;
            this.Name = name;
            this.Artist = artist;
            this.Category = category;
            this.CreationDate = creationDate;
            this.ModificationDate = modificationDate;
            this.OsuModificationDate = osuModificationDate;
        }
    }
}
