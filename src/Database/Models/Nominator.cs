using System.Collections.Generic;

namespace Database.Models
{
    public class Nominator
    {
        public int NominatorId { get; set; }
        public string Name { get; set; }

        public List<Map> Maps { get; set; }

        public Nominator(string name)
        {
            this.Name = name;
        }
    }
}
