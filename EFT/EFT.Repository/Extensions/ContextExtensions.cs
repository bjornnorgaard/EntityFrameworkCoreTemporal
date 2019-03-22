using Microsoft.EntityFrameworkCore.Migrations;

namespace EFT.Repository.Extensions
{
    public static class ContextExtensions
    {
        public static void AddTemporalTableSupport(this MigrationBuilder builder, string tableName, string historyTableSchema)
        {
            builder.Sql($@"ALTER TABLE {tableName} ADD 
                            SysStartTime datetime2(0) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
                            SysEndTime datetime2(0) GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
                            PERIOD FOR SYSTEM_TIME (SysStartTime, SysEndTime);");
            builder.Sql($@"ALTER TABLE {tableName} SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = {historyTableSchema}.{tableName} ));");
        }
    }
}
