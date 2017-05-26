using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sadu.Models
{
    public class Organization
    {
        public Organization()
        {
            this.Pending_Submissions = new List<Submission>();
            this.Members = new List<User>();
        }
        public int Id { get; set; }
        public String name { get; set; }

        public virtual OrganizationImage OrganizationImage { get; set; }
        public virtual ICollection<Submission> Pending_Submissions { get; set; }
        public virtual ICollection<User> Members { get; set; }
    }
}