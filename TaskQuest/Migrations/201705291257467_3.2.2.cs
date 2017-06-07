namespace TaskQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _322 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.feb_feedback", name: "Resposta", newName: "feb_resposta");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.feb_feedback", name: "feb_resposta", newName: "Resposta");
        }
    }
}
