using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI.Models;

namespace ourinv.WebAPI.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
