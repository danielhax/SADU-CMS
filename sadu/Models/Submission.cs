using System;
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
        public int Organization_ID { get; set; }

        [ForeignKey("Organization_ID")]
        public Organization Organization { get; set; }
    }
}