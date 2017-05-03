namespace TastQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "taskquest.arq_arquivos",
                c => new
                    {
                        arq_id = c.Int(nullable: false),
                        arq_nome = c.String(nullable: false, maxLength: 20, unicode: false),
                        arq_caminho = c.String(nullable: false, maxLength: 40, unicode: false),
                        arq_tamanho = c.Int(nullable: false),
                        arq_data_upload = c.DateTime(nullable: false, precision: 0),
                        tsk_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.arq_id)
                .ForeignKey("taskquest.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .Index(t => t.tsk_id);
            
            CreateTable(
                "taskquest.tsk_task",
                c => new
                    {
                        tsk_id = c.Int(nullable: false),
                        qst_id = c.Int(nullable: false),
                        tsk_nome = c.String(nullable: false, maxLength: 45, unicode: false),
                        tsk_status = c.Int(nullable: false),
                        tsk_dificuldade = c.Int(nullable: false),
                        tsk_duracao = c.Time(precision: 2),
                        tsk_data_criacao = c.DateTime(nullable: false, precision: 0),
                        tsk_data_conclusao = c.DateTime(precision: 0),
                        tsk_verificacao = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.tsk_id)
                .ForeignKey("taskquest.qst_quest", t => t.qst_id, cascadeDelete: true)
                .Index(t => t.qst_id);
            
            CreateTable(
                "taskquest.feb_feedback",
                c => new
                    {
                        feb_id = c.Int(nullable: false),
                        tsk_id = c.Int(nullable: false),
                        feb_relatorio = c.String(nullable: false, maxLength: 150, unicode: false),
                        feb_nota = c.Int(),
                        feb_feedback = c.String(maxLength: 150, unicode: false),
                        feb_data_conclusao = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.feb_id, t.tsk_id })
                .ForeignKey("taskquest.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .Index(t => t.tsk_id);
            
            CreateTable(
                "taskquest.pre_precedencia",
                c => new
                    {
                        pre_id = c.Int(nullable: false, identity: true),
                        qst_id_antecedente = c.Int(),
                        tsk_id_antecedente = c.Int(),
                        qst_id_precedente = c.Int(),
                        tsk_id_precedente = c.Int(),
                        usu_id_responsavel_conclusao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.pre_id)
                .ForeignKey("taskquest.usu_usuario", t => t.usu_id_responsavel_conclusao)
                .ForeignKey("taskquest.qst_quest", t => t.qst_id_antecedente, cascadeDelete: true)
                .ForeignKey("taskquest.qst_quest", t => t.qst_id_precedente, cascadeDelete: true)
                .ForeignKey("taskquest.tsk_task", t => t.tsk_id_antecedente, cascadeDelete: true)
                .ForeignKey("taskquest.tsk_task", t => t.tsk_id_precedente, cascadeDelete: true)
                .Index(t => t.qst_id_antecedente)
                .Index(t => t.tsk_id_antecedente)
                .Index(t => t.qst_id_precedente)
                .Index(t => t.tsk_id_precedente)
                .Index(t => t.usu_id_responsavel_conclusao);
            
            CreateTable(
                "taskquest.qst_quest",
                c => new
                    {
                        qst_id = c.Int(nullable: false),
                        usu_id_criador = c.Int(),
                        gru_id_criador = c.Int(),
                        qst_cor = c.String(nullable: false, maxLength: 45, unicode: false),
                        qst_criacao = c.DateTime(nullable: false, precision: 0),
                        qst_descricao = c.String(nullable: false, maxLength: 45, unicode: false),
                        qst_nome = c.String(nullable: false, maxLength: 45, unicode: false),
                    })
                .PrimaryKey(t => t.qst_id)
                .ForeignKey("taskquest.usu_usuario", t => t.usu_id_criador, cascadeDelete: true)
                .ForeignKey("taskquest.gru_grupo", t => t.gru_id_criador, cascadeDelete: true)
                .Index(t => t.usu_id_criador)
                .Index(t => t.gru_id_criador);
            
            CreateTable(
                "taskquest.gru_grupo",
                c => new
                    {
                        gru_id = c.Int(nullable: false),
                        gru_nome = c.String(nullable: false, maxLength: 20, unicode: false),
                        gru_cor = c.String(nullable: false, maxLength: 7, unicode: false),
                        gru_data_criacao = c.DateTime(nullable: false, precision: 0),
                        gru_plano = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.gru_id);
            
            CreateTable(
                "taskquest.msg_mensagem",
                c => new
                    {
                        msg_id = c.Int(nullable: false),
                        usu_id_remetente = c.Int(nullable: false),
                        usu_id_destinatario = c.Int(),
                        gru_id_destinatario = c.Int(),
                        msg_conteudo = c.String(nullable: false, maxLength: 120, unicode: false),
                        msg_data = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.msg_id)
                .ForeignKey("taskquest.usu_usuario", t => t.usu_id_destinatario, cascadeDelete: true)
                .ForeignKey("taskquest.usu_usuario", t => t.usu_id_remetente, cascadeDelete: true)
                .ForeignKey("taskquest.gru_grupo", t => t.gru_id_destinatario, cascadeDelete: true)
                .Index(t => t.usu_id_remetente)
                .Index(t => t.usu_id_destinatario)
                .Index(t => t.gru_id_destinatario);
            
            CreateTable(
                "taskquest.usu_usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        usu_nome = c.String(nullable: false, maxLength: 20, unicode: false),
                        usu_sobrenome = c.String(nullable: false, maxLength: 20, unicode: false),
                        usu_data_nascimento = c.DateTime(nullable: false, storeType: "date"),
                        usu_sexo = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        Email = c.String(unicode: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        LockoutEndDateUtc = c.DateTime(precision: 0),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomUserClaims",
                c => new
                    {
                        CustomUserClaimId = c.Int(nullable: false, identity: true),
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.CustomUserClaimId)
                .ForeignKey("taskquest.usu_usuario", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetClients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientKey = c.String(unicode: false),
                        ApplicationUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("taskquest.usu_usuario", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "taskquest.crt_cartao",
                c => new
                    {
                        crt_id = c.Int(nullable: false, identity: true),
                        usu_usuario_usu_id = c.Int(nullable: false),
                        crt_bandeira = c.String(nullable: false, maxLength: 45, unicode: false),
                        crt_numero_cartao = c.Int(nullable: false),
                        crt_nome_titular = c.String(nullable: false, maxLength: 30, unicode: false),
                        crt_data_vencimento = c.DateTime(nullable: false, storeType: "date"),
                        crt_codigo_seguranca = c.String(nullable: false, maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.crt_id)
                .ForeignKey("taskquest.usu_usuario", t => t.usu_usuario_usu_id, cascadeDelete: true)
                .Index(t => t.usu_usuario_usu_id);
            
            CreateTable(
                "dbo.CustomUserLogins",
                c => new
                    {
                        CustomUserLoginId = c.Int(nullable: false, identity: true),
                        LoginProvider = c.String(unicode: false),
                        ProviderKey = c.String(unicode: false),
                        UserId = c.Int(nullable: false),
                        ApplicationUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.CustomUserLoginId)
                .ForeignKey("taskquest.usu_usuario", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.CustomUserRoles",
                c => new
                    {
                        CustomUserRoleId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        ApplicationUser_Id = c.Int(),
                        CustomRole_CustomRoleId = c.Int(),
                    })
                .PrimaryKey(t => t.CustomUserRoleId)
                .ForeignKey("taskquest.usu_usuario", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.CustomRoles", t => t.CustomRole_CustomRoleId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.CustomRole_CustomRoleId);
            
            CreateTable(
                "taskquest.tel_telefone",
                c => new
                    {
                        tel_id = c.Int(nullable: false, identity: true),
                        usu_id = c.Int(nullable: false),
                        tel_ddd = c.Int(nullable: false),
                        tel_numero = c.Int(nullable: false),
                        tel_tipo = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.tel_id)
                .ForeignKey("taskquest.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id);
            
            CreateTable(
                "taskquest.uxg_usuario_grupo",
                c => new
                    {
                        usu_id = c.Int(nullable: false),
                        gru_id = c.Int(nullable: false),
                        uxg_administrador = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.usu_id, t.gru_id })
                .ForeignKey("taskquest.gru_grupo", t => t.gru_id, cascadeDelete: true)
                .ForeignKey("taskquest.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.gru_id)
                .Index(t => t.usu_id);
            
            CreateTable(
                "taskquest.xpu_experiencia_usuario",
                c => new
                    {
                        usu_id = c.Int(nullable: false),
                        tsk_id = c.Int(nullable: false),
                        xpu_valor = c.Int(nullable: false),
                        xpu_data = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.usu_id, t.tsk_id })
                .ForeignKey("taskquest.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .ForeignKey("taskquest.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.tsk_id)
                .Index(t => t.usu_id);
            
            CreateTable(
                "taskquest.xpg_experiencia_grupo",
                c => new
                    {
                        gru_id = c.Int(nullable: false),
                        tsk_id = c.Int(nullable: false),
                        xpu_valor = c.Int(nullable: false),
                        xpu_data = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.gru_id, t.tsk_id })
                .ForeignKey("taskquest.gru_grupo", t => t.gru_id, cascadeDelete: true)
                .ForeignKey("taskquest.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .Index(t => t.gru_id)
                .Index(t => t.tsk_id);
            
            CreateTable(
                "taskquest.sem_semana",
                c => new
                    {
                        sem_id = c.Int(nullable: false),
                        sem_dia = c.String(nullable: false, maxLength: 10, unicode: false),
                        sem_sigla = c.String(nullable: false, maxLength: 1, unicode: false),
                    })
                .PrimaryKey(t => t.sem_id);
            
            CreateTable(
                "dbo.AspNetClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomRoles",
                c => new
                    {
                        CustomRoleId = c.Int(nullable: false, identity: true),
                        Id = c.Int(nullable: false),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.CustomRoleId);
            
            CreateTable(
                "taskquest.qxs_quest_semana",
                c => new
                    {
                        qst_id = c.Int(nullable: false),
                        sem_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.qst_id, t.sem_id })
                .ForeignKey("taskquest.qst_quest", t => t.qst_id, cascadeDelete: true)
                .ForeignKey("taskquest.sem_semana", t => t.sem_id, cascadeDelete: true)
                .Index(t => t.qst_id)
                .Index(t => t.sem_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomUserRoles", "CustomRole_CustomRoleId", "dbo.CustomRoles");
            DropForeignKey("taskquest.pre_precedencia", "tsk_id_precedente", "taskquest.tsk_task");
            DropForeignKey("taskquest.pre_precedencia", "tsk_id_antecedente", "taskquest.tsk_task");
            DropForeignKey("taskquest.tsk_task", "qst_id", "taskquest.qst_quest");
            DropForeignKey("taskquest.qxs_quest_semana", "sem_id", "taskquest.sem_semana");
            DropForeignKey("taskquest.qxs_quest_semana", "qst_id", "taskquest.qst_quest");
            DropForeignKey("taskquest.pre_precedencia", "qst_id_precedente", "taskquest.qst_quest");
            DropForeignKey("taskquest.pre_precedencia", "qst_id_antecedente", "taskquest.qst_quest");
            DropForeignKey("taskquest.xpg_experiencia_grupo", "tsk_id", "taskquest.tsk_task");
            DropForeignKey("taskquest.xpg_experiencia_grupo", "gru_id", "taskquest.gru_grupo");
            DropForeignKey("taskquest.qst_quest", "gru_id_criador", "taskquest.gru_grupo");
            DropForeignKey("taskquest.msg_mensagem", "gru_id_destinatario", "taskquest.gru_grupo");
            DropForeignKey("taskquest.xpu_experiencia_usuario", "usu_usuario_Id", "taskquest.usu_usuario");
            DropForeignKey("taskquest.xpu_experiencia_usuario", "tsk_id", "taskquest.tsk_task");
            DropForeignKey("taskquest.uxg_usuario_grupo", "usu_usuario_Id", "taskquest.usu_usuario");
            DropForeignKey("taskquest.uxg_usuario_grupo", "gru_id", "taskquest.gru_grupo");
            DropForeignKey("taskquest.tel_telefone", "usu_usuario_Id", "taskquest.usu_usuario");
            DropForeignKey("dbo.CustomUserRoles", "ApplicationUser_Id", "taskquest.usu_usuario");
            DropForeignKey("taskquest.qst_quest", "usu_id_criador", "taskquest.usu_usuario");
            DropForeignKey("taskquest.pre_precedencia", "usu_id_responsavel_conclusao", "taskquest.usu_usuario");
            DropForeignKey("taskquest.msg_mensagem", "usu_id_remetente", "taskquest.usu_usuario");
            DropForeignKey("taskquest.msg_mensagem", "usu_id_destinatario", "taskquest.usu_usuario");
            DropForeignKey("dbo.CustomUserLogins", "ApplicationUser_Id", "taskquest.usu_usuario");
            DropForeignKey("taskquest.crt_cartao", "usu_usuario_usu_id", "taskquest.usu_usuario");
            DropForeignKey("dbo.AspNetClients", "ApplicationUser_Id", "taskquest.usu_usuario");
            DropForeignKey("dbo.CustomUserClaims", "Id", "taskquest.usu_usuario");
            DropForeignKey("taskquest.feb_feedback", "tsk_id", "taskquest.tsk_task");
            DropForeignKey("taskquest.arq_arquivos", "tsk_id", "taskquest.tsk_task");
            DropIndex("taskquest.qxs_quest_semana", new[] { "sem_id" });
            DropIndex("taskquest.qxs_quest_semana", new[] { "qst_id" });
            DropIndex("taskquest.xpg_experiencia_grupo", new[] { "tsk_id" });
            DropIndex("taskquest.xpg_experiencia_grupo", new[] { "gru_id" });
            DropIndex("taskquest.xpu_experiencia_usuario", new[] { "usu_usuario_Id" });
            DropIndex("taskquest.xpu_experiencia_usuario", new[] { "tsk_id" });
            DropIndex("taskquest.uxg_usuario_grupo", new[] { "usu_usuario_Id" });
            DropIndex("taskquest.uxg_usuario_grupo", new[] { "gru_id" });
            DropIndex("taskquest.tel_telefone", new[] { "usu_usuario_Id" });
            DropIndex("dbo.CustomUserRoles", new[] { "CustomRole_CustomRoleId" });
            DropIndex("dbo.CustomUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CustomUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("taskquest.crt_cartao", new[] { "usu_usuario_usu_id" });
            DropIndex("dbo.AspNetClients", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CustomUserClaims", new[] { "Id" });
            DropIndex("taskquest.msg_mensagem", new[] { "gru_id_destinatario" });
            DropIndex("taskquest.msg_mensagem", new[] { "usu_id_destinatario" });
            DropIndex("taskquest.msg_mensagem", new[] { "usu_id_remetente" });
            DropIndex("taskquest.qst_quest", new[] { "gru_id_criador" });
            DropIndex("taskquest.qst_quest", new[] { "usu_id_criador" });
            DropIndex("taskquest.pre_precedencia", new[] { "usu_id_responsavel_conclusao" });
            DropIndex("taskquest.pre_precedencia", new[] { "tsk_id_precedente" });
            DropIndex("taskquest.pre_precedencia", new[] { "qst_id_precedente" });
            DropIndex("taskquest.pre_precedencia", new[] { "tsk_id_antecedente" });
            DropIndex("taskquest.pre_precedencia", new[] { "qst_id_antecedente" });
            DropIndex("taskquest.feb_feedback", new[] { "tsk_id" });
            DropIndex("taskquest.tsk_task", new[] { "qst_id" });
            DropIndex("taskquest.arq_arquivos", new[] { "tsk_id" });
            DropTable("taskquest.qxs_quest_semana");
            DropTable("dbo.CustomRoles");
            DropTable("dbo.AspNetClaims");
            DropTable("taskquest.sem_semana");
            DropTable("taskquest.xpg_experiencia_grupo");
            DropTable("taskquest.xpu_experiencia_usuario");
            DropTable("taskquest.uxg_usuario_grupo");
            DropTable("taskquest.tel_telefone");
            DropTable("dbo.CustomUserRoles");
            DropTable("dbo.CustomUserLogins");
            DropTable("taskquest.crt_cartao");
            DropTable("dbo.AspNetClients");
            DropTable("dbo.CustomUserClaims");
            DropTable("taskquest.usu_usuario");
            DropTable("taskquest.msg_mensagem");
            DropTable("taskquest.gru_grupo");
            DropTable("taskquest.qst_quest");
            DropTable("taskquest.pre_precedencia");
            DropTable("taskquest.feb_feedback");
            DropTable("taskquest.tsk_task");
            DropTable("taskquest.arq_arquivos");
        }
    }
}
