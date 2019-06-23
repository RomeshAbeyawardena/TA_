using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TA.Contracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> NoTrackingQuery { get; }
        IQueryable<T> Query { get; }
        DbSet<T> DbSet { get; }
        DbContext Context { get; }
        Task<T> SaveChangesAsync(T entry, bool commitChanges = true);
    }
}