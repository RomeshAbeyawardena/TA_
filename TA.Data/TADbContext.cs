using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Domains.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TA.Domains.Contracts;
using TA.Domains.Extensions;
using WebToolkit.Contracts.Providers;

namespace TA.Data
{
    public class TADbContext : DbContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        private void SetMetaData<T>(T entity, DateTimeOffset? createdDate = null, DateTimeOffset? modifiedDate = null)
        {
            if (entity is ICreated createdEntity)
                createdEntity.Created = createdDate ?? _dateTimeProvider.Now;

            if (entity is IModified modifiedEntity)
                modifiedEntity.Modified = modifiedDate ?? _dateTimeProvider.Now;
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenPermission> TokenPermissions { get; set; }

        public TADbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) 
            : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public override TEntity Find<TEntity>(params object[] keyValues)
        {
            return FindAsync<TEntity>(keyValues, CancellationToken.None).Result;
            //return base.Find<TEntity>(keyValues);
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

        public override Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        {
            SetMetaData(entity);
            return base.AddAsync(entity, cancellationToken);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            return AddAsync(entity, CancellationToken.None).Result;
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            var keyProperties = entity.GetKeyProperties().ToArray();
            
            //Uses overload if more than key property is defined
            var foundEntity = keyProperties.Length > 1 
                ? Find<TEntity>(keyProperties) 
                : Find<TEntity>(keyProperties.Single());

            //Detaches the entity so the provided entity can be used to update instead
            Entry(foundEntity).State = EntityState.Detached;

            if(foundEntity is ICreated createdFoundEntity) 
                SetMetaData(entity, createdFoundEntity.Created);

            return base.Update(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                mutableEntityType.Relational().TableName = mutableEntityType.Relational()
                    .TableName.Singularize();
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}