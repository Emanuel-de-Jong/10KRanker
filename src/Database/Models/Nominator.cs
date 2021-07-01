namespace Database.Models
{
    public class Nominator
    {
        public int NominatorId { get; set; }
        public string Name { get; set; }

        public Nominator(string name)
        {
            Name = name;
        }
    }
}
