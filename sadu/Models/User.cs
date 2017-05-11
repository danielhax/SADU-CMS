using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace sadu.Models
{
    public class User
    {
        public int ID { get; set;}
        public String username { get; set; }
        private String password { get; set; }
        private bool isAdmin { get; set; }
    }

    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}