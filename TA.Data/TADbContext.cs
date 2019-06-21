using Microsoft.EntityFrameworkCore;
using TA.Domains.Models;

namespace TA.Data
{
    public class TADbContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }

        public TADbContext(DbContextOptions options) 
            : base(options)
        {
            
        }
    }
}