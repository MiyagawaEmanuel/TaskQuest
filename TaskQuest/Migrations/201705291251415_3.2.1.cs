namespace TaskQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _321 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tsk_task", "tsk_data_conclusao", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tsk_task", "tsk_data_conclusao", c => c.DateTime(nullable: false, precision: 0));
        }
    }
}
