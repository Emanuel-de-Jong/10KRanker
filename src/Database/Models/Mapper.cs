namespace Database.Models
{
    public class Mapper
    {
        public int MapperId { get; set; }
        public string Name { get; set; }

        public Mapper(string name)
        {
            Name = name;
        }
    }
}
