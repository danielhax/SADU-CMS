using sadu.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace sadu.DAL
{
    public class SADUInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SADUContext>
    {
        protected override void Seed(SADUContext context)
        {
            //Declare sample orgs first so it can be used in creating users, otherwise the orgs won't exist
            Organization org1 = new Organization { name = "SSC" },
                org2 = new Organization { name = "ACM" },
                org3 = new Organization { name = "JPCS" };

            var users = new List<User>
            {
                new User{username = "johndoe", password = "johnd", firstName = "John", lastName = "Doe", isAdmin = false , Organizations = new List<Organization>(){org1, org2} },
                new User{username = "mariadb", password = "mariadb", firstName = "Maria", lastName = "Debeaux", isAdmin = false, Organizations = new List<Organization>(){org3}},
                new User{username = "keysersoz", password = "keysoz", firstName = "Keyser", lastName = "Soze", isAdmin = false, Organizations = new List<Organization>(){org1}},
                new User{username = "jeanbaljean", password = "jbj1234", firstName = "Jean", lastName = "Baljean", isAdmin = false, Organizations = new List<Organization>(){org2, org3}},
                new User{username = "admin", password = "admin", firstName = "Adminis", lastName = "Traitor", isAdmin = true, Organizations = null},
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var submissions = new List<Submission>
            {
                new Submission{title="Documents", description="Submit these documents", Organization = org1},
                new Submission{title="Documents", description="Submit these documents", Organization = org2},
                new Submission{title="Documents", description="Submit these documents", Organization = org2},
                new Submission{title="Documents", description="Submit these documents", Organization = org3},
                new Submission{title="Documents", description="Submit these documents", Organization = org3}
            };

            submissions.ForEach(s => context.Submissions.Add(s));
            context.SaveChanges();

            //extra code for adding sample submissions to orgs
            org1.Pending_Submissions.Add(submissions[0]);
            org2.Pending_Submissions.AddRange(submissions.GetRange(1, 2));
            org3.Pending_Submissions.AddRange(submissions.GetRange(3, 1));

            var organizations = new List<Organization>
            {
                org1,
                org2,
                org3
            };

            organizations.ForEach(o => context.Organizations.Add(o));
            context.SaveChanges();

        }
    }
}