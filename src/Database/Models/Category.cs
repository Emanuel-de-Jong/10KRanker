using System.Collections.Generic;

namespace Database.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public List<Map> Maps { get; set; }

        public Category(string name)
        {
            this.Name = name;
        }

        public Category(int id, string name)
        {
            this.CategoryId = id;
            this.Name = name;
        }

        public static void Init(Context db)
        {
            db.AddRange(new Category[]
            {
                new Category(-2, "Graveyard"),
                new Category(-1, "WIP"),
                new Category(0, "Pending"),
                new Category(1, "Ranked"),
                new Category(2, "Approved"),
                new Category(3, "Qualified"),
                new Category(4, "Loved"),
            });
            db.SaveChanges();
        }
    }
}
