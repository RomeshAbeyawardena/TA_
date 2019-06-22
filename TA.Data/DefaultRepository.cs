using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
using TA.Domains.Models;

namespace TA.Data
{
    public class DefaultRepository<TDbContext, T> : IRepository<T> 
        where TDbContext : DbContext
        where T : class
    {
        private readonly TDbContext _dbContext;
        public IQueryable<T> NoTrackingQuery => DbSet.AsNoTracking();
        public IQueryable<T> Query => DbSet;
        public DbSet<T> DbSet => _dbContext.Set<T>();
        
        public DbContext Context => _dbContext;
        public async Task<T> SaveChangesAsync<TProp>(T entry, 
            Func<T, TProp> updateIdentityExpression, bool commitChanges = true)
        {
            if (updateIdentityExpression(entry) == null) 
                await DbSet.AddAsync(entry);
            else
                DbSet.Update(entry);
            
            if (commitChanges)
                await _dbContext.SaveChangesAsync();

            return entry;
        }

        public DefaultRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}