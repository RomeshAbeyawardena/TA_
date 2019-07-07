using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Domains.Models;
using WebToolkit.Common;
using WebToolkit.Contracts.Providers;

namespace TA.Data
{
    public class TADbContext : ExtendedDbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenPermission> TokenPermissions { get; set; }
        public DbSet<User> User { get; set; }
        public TADbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) 
            : base(options, dateTimeProvider)
        {
        }

        public override TEntity Find<TEntity>(params object[] keyValues)
        {
            return FindAsync<TEntity>(keyValues, CancellationToken.None).Result;
        }

        public override async Task<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken)
        {
            var value = await base.FindAsync<TEntity>(keyValues, cancellationToken);

            Entry(value).State = EntityState.Detached;

            return value;
        }

        public override async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        {
            return await FindAsync<TEntity>(keyValues, CancellationToken.None);
        }

    }
}