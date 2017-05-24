using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sadu.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public String title { get; set; }
        public String description { get; set; }


        //public virtual List<User> Members { get; set; }
    }
}