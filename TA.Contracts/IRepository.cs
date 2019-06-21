using Microsoft.EntityFrameworkCore;

namespace TA
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> DbSet { get; }
    }
}