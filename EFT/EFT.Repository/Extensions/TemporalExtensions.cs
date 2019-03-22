using System;
using System.Linq;
using System.Reflection;
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

            var query = "SELECT * FROM [{0}].[{1}] FOR SYSTEM_TIME BETWEEN '{2}' AND '{3}' ";

            return source.FromSql(query, mapping.Schema, mapping.TableName, fromDate, toDate);
        }

        public static IQueryable<TEntity> Between<TEntity>(this IQueryable<TEntity> source, DateTime from, DateTime to) where TEntity : class
        {
            var mapping = source.GetDbContext().Model.FindEntityType(typeof(TEntity)).Relational();

            var query = "SELECT * FROM [{0}].[{1}] FOR SYSTEM_TIME BETWEEN '{2}' AND '{3}' ";

            return source.FromSql(query, mapping.Schema, mapping.TableName, from.ToString("yyyy/MM/dd HH:mm"), to.ToString("yyyy/MM/dd HH:mm"));
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
    }
}
