using FluentMigrator;

namespace Vulnerable.Sample.Migration.Migrations 
{
    [Migration(201907141255)]
    public class InsertPeople : FluentMigrator.Migration
    {
        public override void Up()
        {
            Insert.IntoTable("People").Row(new {
                Name = "a",
            }).Row(new {
                Name = "b",
            });
        }

        public override void Down()
        {
            Delete.FromTable("People").AllRows();
        }
    }
}
