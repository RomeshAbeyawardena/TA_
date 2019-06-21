using Microsoft.EntityFrameworkCore;

namespace TA.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TADbContext _dbContext;

        public Repository(TADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<T> DbSet => _dbContext.Set<T>();
    }
}