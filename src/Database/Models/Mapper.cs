using System.Collections.Generic;

namespace Database.Models
{
    public class Mapper
    {
        public int MapperId { get; set; }
        public string Name { get; set; }

        public List<Map> Maps { get; set; }

        public Mapper(string name)
        {
            this.Name = name;
        }
    }
}
