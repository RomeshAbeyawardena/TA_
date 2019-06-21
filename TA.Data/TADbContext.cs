using Microsoft.EntityFrameworkCore;
using TA.Domains.Models;
using Humanizer;
namespace TA.Data
{
    public class TADbContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Site> Sites { get; set; }

        public TADbContext(DbContextOptions options) 
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                mutableEntityType.Relational().TableName = mutableEntityType.Relational().TableName.Singularize();
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}