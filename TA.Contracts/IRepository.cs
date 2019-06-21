using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TA.Contracts
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> DbSet { get; }
        DbContext Context { get; }
        Task<T> SaveChangesAsync<TProp>(T entry, Func<T, TProp> updateIdentityExpression, bool commitChanges = true);
    }
}