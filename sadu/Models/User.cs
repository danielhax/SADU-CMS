using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadu.Models
{
    public class User
    {

        public User()
        {
            this.Organizations = new List<Organization>();
        }
        
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Index(IsUnique=true)]
        public String email { get; set; }
        [Required]
        public String password { get; set; }
        [Required]
        public String firstName { get; set; }
        [Required]
        public String lastName { get; set; }
        [Required]
        public bool isAdmin { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }
    }
}