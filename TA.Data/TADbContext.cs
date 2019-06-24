using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Domains.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TA.Contracts;
using TA.Domains.Contracts;
using TA.Domains.Extensions;

namespace TA.Data
{
    public class TADbContext : DbContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenPermission> TokenPermissions { get; set; }

        public TADbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) 
            : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public override Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        {
            if (entity is ICreated createdEntity)
                createdEntity.Created = _dateTimeProvider.Now;

            if (entity is IModified modifiedEntity)
                modifiedEntity.Modified = _dateTimeProvider.Now;

            return base.AddAsync(entity, cancellationToken);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            return AddAsync(entity, CancellationToken.None).Result;
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            var keyProperties = entity.GetKeyProperties().ToArray();
            
            var foundEntity = keyProperties.Length > 1 
                ? Find<TEntity>(keyProperties) 
                : Find<TEntity>(keyProperties.Single());

            Entry(foundEntity).State = EntityState.Detached;

            if (entity is ICreated createdEntity && foundEntity is ICreated createdFoundEntity)
                createdEntity.Created = createdFoundEntity.Created;

            if (entity is IModified modifiedEntity)
                modifiedEntity.Modified = _dateTimeProvider.Now;

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