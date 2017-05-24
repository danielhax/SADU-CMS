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
        }
        public int Id { get; set; }
        public String name { get; set; }
        public virtual List<Submission> Pending_Submissions { get; set; }
        public virtual List<User> Members { get; set; }
    }
}