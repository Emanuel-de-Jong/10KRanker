using Database.Models;
using System;
using System.Linq;

namespace Database
{
    public class DB
    {
        public DB()
        {
            using (var db = new Context())
            {
                // Create
                Console.WriteLine("Inserting a new blog");
                db.Add(new Nominator("Nominator 1"));
                db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a blog");
                var nominator = db.Nominators
                    .OrderBy(n => n.NominatorId)
                    .First();

                // Update
                Console.WriteLine("Updating the blog and adding a post");
                nominator.Name = "Nominator 11";
                db.SaveChanges();

                // Delete
                Console.WriteLine("Delete the blog");
                db.Remove(nominator);
                db.SaveChanges();
            }
        }
    }
}
