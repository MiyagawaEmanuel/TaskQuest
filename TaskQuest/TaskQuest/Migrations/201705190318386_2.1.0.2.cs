namespace TaskQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2102 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.arq_arquivos", "arq_versao_atual", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.arq_arquivos", "arq_versao_atual");
        }
    }
}
