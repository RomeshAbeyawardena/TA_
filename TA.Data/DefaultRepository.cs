using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Domains.Extensions;
using WebToolkit.Contracts.Data;
using WebToolkit.Contracts.Providers;

namespace TA.Data
{
    public class DefaultRepository<TDbContext, T> : IRepository<T> 
        where TDbContext : DbContext
        where T : class
    {
        private readonly IDefaultValueProvider<T> _defaultValueProvider;

        public T Attach(T entity)
        {
            return DbSet.Attach(entity).Entity;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> queryExpression = null, bool trackingQuery = false)
        {
            var query = trackingQuery
                ? DbSet
                : DbSet.AsNoTracking();

            return queryExpression == null 
                ? query
                : query.Where(queryExpression);
        }

        public DbSet<T> DbSet => Context.Set<T>();
        
        public DbContext Context { get; }
        public async Task<T> SaveChangesAsync(T entry, bool commitChanges = true)
        {
            _defaultValueProvider.Assign(entry);

            var entries = entry.GetKeyProperties();
            if (entries.All(e => e.IsDefault())) 
                await DbSet.AddAsync(entry);
            else
                DbSet.Update(entry);
            
            if (commitChanges)
                await Context.SaveChangesAsync();

            return entry;
        }

        public DefaultRepository(TDbContext dbContext, IDefaultValueProvider<T> defaultValueProvider)
        {
            _defaultValueProvider = defaultValueProvider;
            Context = dbContext;
        }
    }
}