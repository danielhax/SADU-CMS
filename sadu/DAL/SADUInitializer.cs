using sadu.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace sadu.DAL
{
    public class SADUInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SADUContext>
    {
        protected override void Seed(SADUContext context)
        {
            var users = new List<User>
            {
                new User{username="johndoe", password="johnd", firstName="John", lastName="Doe", isAdmin = false },
                new User{username="mariadb",password="mariadb", firstName="Maria", lastName="Debeaux", isAdmin=false},
                new User{username="keysersoz",password="keysoz", firstName="Keyser", lastName="Soze", isAdmin=false},
                new User{username="jeanbaljean",password="jbj1234", firstName="Jean", lastName="Baljean", isAdmin=false},
                new User{username="admin",password="admin", firstName="Adminis", lastName="Traitor", isAdmin=true},
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var organizations = new List<Organization>
            {
                new Organization{name="SSC"}
            };

            organizations.ForEach(o => context.Organizations.Add(o));
            context.SaveChanges();
        }
    }
}