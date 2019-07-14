using FluentMigrator;

namespace Vulnerable.Sample.Migration.Migrations 
{
    [Migration(201907131255)]
    public class AddPeopleTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("People")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString();
        }

        public override void Down()
        {
            Delete.Table("People");
        }
    }
}
