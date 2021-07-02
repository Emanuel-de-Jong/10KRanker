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
                db.Add(new Nominator("Nominator 1"));
                db.SaveChanges();

                // Read
                var nominator = db.Nominators
                    .OrderBy(n => n.NominatorId)
                    .First();

                // Update
                nominator.Name = "Nominator 11";
                db.SaveChanges();

                // Delete
                db.Remove(nominator);
                db.SaveChanges();
            }
        }
    }
}
