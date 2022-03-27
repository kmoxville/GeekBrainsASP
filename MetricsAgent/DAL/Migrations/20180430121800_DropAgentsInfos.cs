using FluentMigrator;

namespace MetricsAgent.DAL.Migrations
{
    [Migration(20180430121800)]
    public class DropAgentsInfos : Migration
    {
        public override void Down()
        {
            Create.Table("AgentInfos")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Id").AsString()
                .WithColumn("Id").AsBoolean();
        }

        public override void Up()
        {
            Delete.Table("AgentInfos");
        }
    }
}
