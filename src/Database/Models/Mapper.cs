using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Database
{
    [Index(nameof(Name), IsUnique = true)]
    public class Mapper
    {
        public long MapperId { get; set; }
        public string Name { get; set; }

        public List<Map> Maps { get; set; }

        public Mapper(long mapperId, string name)
        {
            this.MapperId = mapperId;
            this.Name = name;
        }
    }
}
