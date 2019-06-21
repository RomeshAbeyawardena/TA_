using Microsoft.EntityFrameworkCore;

namespace TA.Contracts
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> DbSet { get; }
    }
}