using System;
using System.Collections.Generic;
using sadu.Models;

namespace sadu.Controllers
{
    /*
     * 
        This class contains common functions used across different controllers
    */
    public class Functions
    {
        public List<Submission> getPendingSubmissions(List<Organization> organizations)
        {
            List<Submission> submissions = new List<Submission>();
            //loop through Model which contains each organization of the current user
            foreach (var org in organizations)
            {
                //loop through submissions of each org
                foreach (var sub in org.Pending_Submissions)
                {
                    submissions.Add(sub);
                }
            }

            return submissions;
        }
        public IEnumerable<Submission> filterSubmissionsByOrg(List<Organization> organizations, String orgName)
        {
            List<Submission> sorted = new List<Submission>();

            //loop through Model which contains each organization of the current user
            foreach (var org in organizations)
            {
                //if certain org is found
                if (org.name.Equals(orgName))
                {
                    //loop through submissions of each org then add to sorted list
                    foreach (var sub in org.Pending_Submissions)
                    {
                        sorted.Add(sub);
                    }
                }

            }

            return sorted;
        }
    }
}