using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadu.Models
{
    public class Submission
    {
        public int Id { get; set; }
        [Required]
        public String title { get; set; }
        public String description { get; set; }
        [Required]
        public virtual Organization Organization { get; set; }
        
    }
}