using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogSiteFinal.Models
{
    public class BlogDbContext:DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<BlogItemsModel> BlogItems { get; set; }
        public DbSet<Admin> Admin { get; set; }
      
    }
}
