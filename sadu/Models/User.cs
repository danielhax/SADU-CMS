using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sadu.Models
{
    public class User
    {
        
        public int Id { get; set; }
        [Required]
        public String username { get; set; }
        [Required]
        public String password { get; set; }
        [Required]
        public String firstName { get; set; }
        [Required]
        public String lastName { get; set; }
        [Required]
        public bool isAdmin { get; set; }

        public virtual List<Organization> Organizations { get; set;}
    }
}