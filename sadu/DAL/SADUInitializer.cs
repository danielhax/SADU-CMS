using sadu.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace sadu.DAL
{
    /*
        THIS CLASS CONTAINS SAMPLE DATA
        THIS CLASS IS ONLY TEMPORARY
    */
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
                new User{username = "johndoe", password = "johnd", firstName = "John", lastName = "Doe", isAdmin = false,},
                new User{username = "mariadb", password = "mariadb", firstName = "Maria", lastName = "Debeaux", isAdmin = false},
                new User{username = "keysersoz", password = "keysoz", firstName = "Keyser", lastName = "Soze", isAdmin = false},
                new User{username = "jeanbaljean", password = "jbj1234", firstName = "Jean", lastName = "Baljean", isAdmin = false},
                new User{username = "admin", password = "admin", firstName = "Adminis", lastName = "Traitor", isAdmin = true, Organizations = null},
            };

            var submissions = new List<Submission>
            {
                new Submission{title="Documents", details="Submit these documents", date_created = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), date_submitted = null, approved = false},
                new Submission{title="Documents2", details="Submit these documents", date_created = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), date_submitted = null, approved = false},
                new Submission{title="Documents3", details="Submit these documents", date_created = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), date_submitted = null, approved = false},
                new Submission{title="Documents4", details="Submit these documents", date_created = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), date_submitted = null, approved = false},
                new Submission{title="Documents5", details="Submit these documents", date_created = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), date_submitted = null, approved = false}
            };

            var images = new List<OrganizationImage>
            {                                  //get image file name and store to database
                new OrganizationImage{Image = Path.GetFileName("~//Assets//acm.png")},
                new OrganizationImage{Image = Path.GetFileName("~//Assets//jpcs.jpg")},
                new OrganizationImage{Image = Path.GetFileName("~//Assets//scc.png")}
            };


            //adding users to orgs

            org1.Members.Add(users[0]);
            org2.Members.Add(users[0]);
            org2.Members.Add(users[1]);
            org3.Members.Add(users[1]);
            org1.Members.Add(users[2]);
            org3.Members.Add(users[2]);
            org1.Members.Add(users[3]);

            //adding images to orgs
            org1.OrganizationImage = images[0];
            org2.OrganizationImage = images[1];
            org3.OrganizationImage = images[2];

            //adding sample submissions to orgs
            org1.Pending_Submissions.Add(submissions[0]);
            org2.Pending_Submissions.Add(submissions[1]);
            org2.Pending_Submissions.Add(submissions[2]);
            org3.Pending_Submissions.Add(submissions[3]);
            org3.Pending_Submissions.Add(submissions[4]);

            var organizations = new List<Organization>
            {
                org1,
                org2,
                org3
            };

            //INSERT ORG FIRST TO GENERATE KEYS?
            organizations.ForEach(o => context.Organizations.Add(o));
            //INSERT USERS
            users.ForEach(u => context.Users.Add(u));
            //INSERT IMAGES
            images.ForEach(i => context.Organization_Images.Add(i));
            //INSERT SUBMISSIONS
            submissions.ForEach(s => context.Submissions.Add(s));

            context.SaveChanges();

        }
    }
}