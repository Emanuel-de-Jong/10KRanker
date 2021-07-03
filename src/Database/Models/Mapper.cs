using System.Collections.Generic;

namespace Database
{
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
