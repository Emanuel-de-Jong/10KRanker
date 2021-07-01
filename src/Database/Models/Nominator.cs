using Microsoft.EntityFrameworkCore;

namespace Database.Models
{
    public class Nominator : CRUD
    {
        public new static DbSet<Nominator> DBSet = Context.Nominators;

        public int Id { get; set; }
        public string Name { get; set; }

        public Nominator(string name)
        {
            Name = name;
        }
    }
}
