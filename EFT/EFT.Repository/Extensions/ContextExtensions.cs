using EFT.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFT.Repository.Extensions
{
    public static class ContextExtensions
    {
        public static void AddTemporalTableSupport(this MigrationBuilder builder, string tableName, string historyTableSchema)
        {
            builder.Sql($@"ALTER TABLE {tableName} ADD 
                            SysStartTime datetime2(0) GENERATED ALWAYS AS ROW START NOT NULL,
                            SysEndTime datetime2(0) GENERATED ALWAYS AS ROW END NOT NULL,
                            PERIOD FOR SYSTEM_TIME (SysStartTime, SysEndTime);");
            builder.Sql($@"ALTER TABLE {tableName} SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = {historyTableSchema}.{tableName} ));");
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
