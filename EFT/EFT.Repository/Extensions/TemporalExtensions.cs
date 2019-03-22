using System;
using System.Linq;
using System.Reflection;
using EFT.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EFT.Repository.Extensions
{
    public static class TemporalExtensions
    {
        public static IQueryable<TEntity> Between<TEntity>(this IQueryable<TEntity> source, long from, long to) where TEntity : class
        {
            var fromDate = DateTimeOffset.FromUnixTimeMilliseconds(from).UtcDateTime.ToString("yyyy/MM/dd HH:mm");
            var toDate = DateTimeOffset.FromUnixTimeMilliseconds(to).UtcDateTime.ToString("yyyy/MM/dd HH:mm");

            var mapping = source.GetDbContext().Model.FindEntityType(typeof(TEntity)).Relational();

            var query = $"SELECT * FROM [dbo].[{mapping.TableName}] FOR SYSTEM_TIME BETWEEN '{fromDate}' AND '{toDate}' ";

            return source.FromSql(query, mapping.TableName);
        }

        public static IQueryable<TEntity> Between<TEntity>(this IQueryable<TEntity> source, DateTime from, DateTime to) where TEntity : class
        {
            var mapping = source.GetDbContext().Model.FindEntityType(typeof(TEntity)).Relational();

            var query = $"SELECT * FROM [dbo].[{mapping.TableName}] " +
                        $"FOR SYSTEM_TIME BETWEEN '{from.ToString("yyyy/MM/dd HH:mm")}' AND '{to.ToString("yyyy/MM/dd HH:mm")}' ";

            return source.FromSql(query);
        }

        private static DbContext GetDbContext(this IQueryable query)
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var queryCompiler = typeof(EntityQueryProvider).GetField("_queryCompiler", bindingFlags).GetValue(query.Provider);
            var queryContextFactory = queryCompiler.GetType().GetField("_queryContextFactory", bindingFlags).GetValue(queryCompiler);

            var dependencies = typeof(RelationalQueryContextFactory).GetProperty("Dependencies", bindingFlags).GetValue(queryContextFactory);
            var queryContextDependencies = typeof(DbContext).Assembly.GetType(typeof(QueryContextDependencies).FullName);
            var stateManagerProperty = queryContextDependencies.GetProperty("StateManager", bindingFlags | BindingFlags.Public).GetValue(dependencies);
            var stateManager = (IStateManager)stateManagerProperty;
            stateManager = stateManager ?? (((LazyRef<IStateManager>)stateManagerProperty)?.Value ?? ((dynamic)stateManagerProperty).Value);

            return stateManager.Context;
        }

        public static ModelBuilder SetupTemporalEntities<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : TemporalEntity
        {
            modelBuilder
                .Entity<TEntity>()
                .Property(e => e.SysEndTime)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.BeforeSaveBehavior = Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore;

            modelBuilder
                .Entity<TEntity>()
                .Property(e => e.SysStartTime)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.AfterSaveBehavior = Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore;

            modelBuilder
                .Entity<TEntity>()
                .Property(e => e.SysEndTime)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.AfterSaveBehavior = Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore;

            modelBuilder
                .Entity<TEntity>()
                .Property(e => e.SysStartTime)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.BeforeSaveBehavior = Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore;

            return modelBuilder;
        }
    }
}
