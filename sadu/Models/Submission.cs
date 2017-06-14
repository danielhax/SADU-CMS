using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sadu.Models
{
    public class Submission
    {
        public Submission()
        {
            this.archived = false;
        }

        public int Id { get; set; }
        [Required]
        public String title { get; set; }
        public String details { get; set; }
        [Required]
        public String date_created { get; set; }
        [Required]
        public String date_deadline { get; set; }
        public bool archived { get; set; }
        
    }
}