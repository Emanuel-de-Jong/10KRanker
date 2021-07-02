using System.Collections.Generic;

namespace Database.Models
{
    public class Nominator
    {
        public long NominatorId { get; set; }
        public string Name { get; set; }

        public List<Map> Maps { get; set; }

        public Nominator(long nominatorId, string name)
        {
            this.NominatorId = nominatorId;
            this.Name = name;
        }
    }
}
