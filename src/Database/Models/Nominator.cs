using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Database
{
    [Index(nameof(Name), IsUnique = true)]
    public class Nominator : IDBModel
    {
        public long NominatorId { get; set; }
        public string Name { get; set; }

        public List<Map> Maps { get; set; }

        public Nominator(long nominatorId, string name)
        {
            this.NominatorId = nominatorId;
            this.Name = name;
        }

        public long GetId()
        {
            return NominatorId;
        }

        public override string ToString()
        {
            return $"new Nominator({NominatorId}, \"{Name}\");";
        }
    }
}
