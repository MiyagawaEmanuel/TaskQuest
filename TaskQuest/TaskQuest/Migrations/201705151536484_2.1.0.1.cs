namespace TaskQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2101 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.gru_grupo", name: "gru_data_cricao", newName: "gru_data_criacao");
            AddColumn("dbo.tsk_task", "tsk_cor", c => c.String(unicode: false));
            AlterColumn("dbo.qst_quest", "qst_cor", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.gru_grupo", "gru_cor", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.gru_grupo", "gru_cor", c => c.String(nullable: false, maxLength: 7, unicode: false));
            AlterColumn("dbo.qst_quest", "qst_cor", c => c.String(nullable: false, maxLength: 45, unicode: false));
            DropColumn("dbo.tsk_task", "tsk_cor");
            RenameColumn(table: "dbo.gru_grupo", name: "gru_data_criacao", newName: "gru_data_cricao");
        }
    }
}
