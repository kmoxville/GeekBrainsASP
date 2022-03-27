using FluentMigrator;
using Microsoft.Data.Sqlite;

namespace MetricsAgent.DAL.Migrations
{
    [Migration(20180430121811)]
    public class CreateSettingsTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Settings");
        }

        public override void Up()
        {
            Create.Table("Settings").WithColumn("Id").AsString().PrimaryKey()
                .WithColumn("Value").AsString();
        }
    }
}
