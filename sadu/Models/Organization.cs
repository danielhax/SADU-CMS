using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sadu.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public String name { get; set; }

        public virtual List<User> Users { get; set; }
    }
}