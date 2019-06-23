using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Domains.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TA.Contracts;
using TA.Domains.Contracts;

namespace TA.Data
{
    public class TADbContext : DbContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Site> Sites { get; set; }

        public TADbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) 
            : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public override Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        {
            if (entity is ICreated createdEntity)
                createdEntity.Created = _dateTimeProvider.DateTimeOffSet;

            if (entity is IModified modifiedEntity)
                modifiedEntity.Modified = _dateTimeProvider.DateTimeOffSet;

            return base.AddAsync(entity, cancellationToken);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            return AddAsync(entity, CancellationToken.None).Result;
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            if (entity is IModified modifiedEntity)
                modifiedEntity.Modified = _dateTimeProvider.DateTimeOffSet;

            return base.Update(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                mutableEntityType.Relational().TableName = mutableEntityType.Relational().TableName.Singularize();
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}