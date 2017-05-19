using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sadu.Models
{
    public class User
    {
        public int Id { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public bool isAdmin { get; set; }

        //public virtual List<Organization> Organizations { get; set;}
    }
}