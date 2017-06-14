using System.Data.Entity;
using sadu.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace sadu.DAL
{
    public class SADUContext : DbContext
    {
        public SADUContext() : base("SADUContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Submission> Submittals { get; set; }
        public DbSet<OrganizationImage> Organization_Images { get; set; }
        
    }
}