using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Database
{
    [Index(nameof(Name), IsUnique = true)]
    public class Mapper : IDBModel
    {
        public long MapperId { get; set; }
        public string Name { get; set; }

        public List<Map> Maps { get; set; }

        public Mapper(long mapperId, string name)
        {
            this.MapperId = mapperId;
            this.Name = name;
        }

        public long GetId()
        {
            return MapperId;
        }

        public override string ToString()
        {
            return $"new Mapper({MapperId}, \"{Name}\")";
        }
    }
}
