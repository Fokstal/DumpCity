using DumpCity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DumpCity.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Category>? Category { get; set; }
        public DbSet<AppType>? AppType { get; set; }
        public DbSet<Product>? Product { get; set; }
        public DbSet<AppUser>? AppUser { get; set; }

    }
}
