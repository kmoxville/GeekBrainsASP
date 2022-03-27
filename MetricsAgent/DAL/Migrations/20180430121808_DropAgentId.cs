using FluentMigrator;
using Microsoft.Data.Sqlite;

namespace MetricsAgent.DAL.Migrations
{
    [Migration(20180430121808)]
    public class DropAgentId : Migration
    {
        public override void Down()
        {
            Alter.Table("CpuMetrics").AddColumn("AgentId").AsInt64();
        }

        public override void Up()
        {
            Execute.Sql("ALTER TABLE CpuMetrics DROP COLUMN AgentId");
        }
    }
}
