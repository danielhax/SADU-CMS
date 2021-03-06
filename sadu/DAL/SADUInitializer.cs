﻿using sadu.Models;
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
            Organization org1 = new Organization { name = "ACM" },
                org2 = new Organization { name = "JPCS" },
                org3 = new Organization { name = "SCC" };

            var users = new List<User>
            {
                new User{email = "johndoe@gmail.com", password = "johnd", firstName = "John", lastName = "Doe", isAdmin = false,},
                new User{email = "mariadb@gmail.com", password = "mariadb", firstName = "Maria", lastName = "Debeaux", isAdmin = false},
                new User{email = "keysersoz@gmail.com", password = "keysoz", firstName = "Keyser", lastName = "Soze", isAdmin = false},
                new User{email = "jeanbaljean@gmail.com", password = "jbj1234", firstName = "Jean", lastName = "Baljean", isAdmin = false},
                new User{email = "admin@gmail.com", password = "admin", firstName = "Adminis", lastName = "Traitor", isAdmin = true, Organization = null},
            };

            var submissions = new List<Submission>
            {
                new Submission{title="Documents", details="Submit these documents", date_created = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), date_deadline = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture)},
                new Submission{title="Documents2", details="Submit these documents", date_created = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), date_deadline = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture)},
                new Submission{title="Documents3", details="Submit these documents", date_created = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), date_deadline = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture)}
            };

            var images = new List<OrganizationImage>
            {                                  //get image file name and store to database
                new OrganizationImage{Image = Path.GetFileName("~//Assets//acm.png"), SidebarImage = @"\\Assets\\Banners\\ACM.png"},
                new OrganizationImage{Image = Path.GetFileName("~//Assets//jpcs.jpg"), SidebarImage = @"\\Assets\\Banners\\JPCS.png"},
                new OrganizationImage{Image = Path.GetFileName("~//Assets//scc.png"), SidebarImage = @"\\Assets\\Banners\\SCC.png"}
            };


            //adding users to orgs

            org1.Members.Add(users[0]);
            org2.Members.Add(users[1]);
            org3.Members.Add(users[2]);
            org1.Members.Add(users[3]);

            //adding images to orgs
            org1.OrganizationImage = images[0];
            org2.OrganizationImage = images[1];
            org3.OrganizationImage = images[2];

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