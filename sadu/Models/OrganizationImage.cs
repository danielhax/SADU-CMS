namespace sadu.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;

    public class OrganizationImage : DbContext
    {
        [Key]
        [ForeignKey("Organization")]
        public int Id { get; set; }

        public String Image { get; set; }

        public virtual Organization Organization { get; set; }
    }
}