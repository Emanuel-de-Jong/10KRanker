using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Database
{
    [Index(nameof(Name), IsUnique = true)]
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
