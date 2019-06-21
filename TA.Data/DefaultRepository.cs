using Microsoft.EntityFrameworkCore;
using TA.Contracts;

namespace TA.Data
{
    public class DefaultRepository<TDbContext, T> : IRepository<T> 
        where TDbContext : DbContext
        where T : class
    {
        private readonly TDbContext _dbContext;
        public DbSet<T> DbSet => _dbContext.Set<T>();

        public DefaultRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}