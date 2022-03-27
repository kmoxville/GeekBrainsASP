using FluentMigrator;
using Microsoft.Data.Sqlite;

namespace MetricsAgent.DAL.Migrations
{
    [Migration(20180430121809)]
    public class AlterTime : Migration
    {
        public override void Down()
        {
            Alter.Table("CpuMetrics").AlterColumn("Time").AsDateTimeOffset();
        }

        public override void Up()
        {
            Alter.Table("CpuMetrics").AlterColumn("Time").AsDateTime();
        }
    }
}
