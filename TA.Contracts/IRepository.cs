using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TA.Contracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query(Expression<Func<T, bool>> queryExpression = null, bool noTrackingQuery = true);
        DbSet<T> DbSet { get; }
        DbContext Context { get; }
        Task<T> SaveChangesAsync(T entry, bool commitChanges = true);
    }
}