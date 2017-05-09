namespace TaskQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.arq_arquivos",
                c => new
                    {
                        arq_id = c.Int(nullable: false, identity: true),
                        arq_nome = c.String(nullable: false, maxLength: 20, unicode: false),
                        arq_caminho = c.String(nullable: false, maxLength: 40, unicode: false),
                        arq_tamanho = c.Int(nullable: false),
                        arq_data_upload = c.DateTime(nullable: false, precision: 0),
                        tsk_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.arq_id)
                .ForeignKey("dbo.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .Index(t => t.tsk_id);
            
            CreateTable(
                "dbo.tsk_task",
                c => new
                    {
                        tsk_id = c.Int(nullable: false, identity: true),
                        qst_id = c.Int(nullable: false),
                        tsk_nome = c.String(nullable: false, maxLength: 45, unicode: false),
                        tsk_status = c.Int(nullable: false),
                        tsk_dificuldade = c.Int(nullable: false),
                        tsk_duracao = c.Time(nullable: false, precision: 2),
                        tsk_data_cricacao = c.DateTime(nullable: false, precision: 0),
                        tsk_data_conclusao = c.DateTime(precision: 0),
                        tsk_verificacao = c.Boolean(nullable: false),
                        usu_id_responsavel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.tsk_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_responsavel)
                .ForeignKey("dbo.qst_quest", t => t.qst_id, cascadeDelete: true)
                .Index(t => t.qst_id)
                .Index(t => t.usu_id_responsavel);
            
            CreateTable(
                "dbo.pre_precedencia",
                c => new
                    {
                        pre_id = c.Int(nullable: false, identity: true),
                        qst_id_antecedente = c.Int(),
                        tsk_id_antecedente = c.Int(),
                        qst_id_precedente = c.Int(),
                        tsk_id_precedente = c.Int(),
                    })
                .PrimaryKey(t => t.pre_id)
                .ForeignKey("dbo.qst_quest", t => t.qst_id_antecedente, cascadeDelete: true)
                .ForeignKey("dbo.qst_quest", t => t.qst_id_precedente, cascadeDelete: true)
                .ForeignKey("dbo.tsk_task", t => t.tsk_id_antecedente, cascadeDelete: true)
                .ForeignKey("dbo.tsk_task", t => t.tsk_id_precedente, cascadeDelete: true)
                .Index(t => t.qst_id_antecedente)
                .Index(t => t.tsk_id_antecedente)
                .Index(t => t.qst_id_precedente)
                .Index(t => t.tsk_id_precedente);
            
            CreateTable(
                "dbo.qst_quest",
                c => new
                    {
                        qst_id = c.Int(nullable: false, identity: true),
                        usu_id_criador = c.Int(),
                        gru_id_criador = c.Int(),
                        qst_cor = c.String(nullable: false, maxLength: 45, unicode: false),
                        qst_criacao = c.DateTime(nullable: false, precision: 0),
                        qst_descricao = c.String(nullable: false, maxLength: 45, unicode: false),
                        qst_nome = c.String(nullable: false, maxLength: 45, unicode: false),
                    })
                .PrimaryKey(t => t.qst_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_criador, cascadeDelete: true)
                .ForeignKey("dbo.gru_grupo", t => t.gru_id_criador, cascadeDelete: true)
                .Index(t => t.usu_id_criador)
                .Index(t => t.gru_id_criador);
            
            CreateTable(
                "dbo.gru_grupo",
                c => new
                    {
                        gru_id = c.Int(nullable: false, identity: true),
                        gru_nome = c.String(nullable: false, maxLength: 20, unicode: false),
                        gru_cor = c.String(nullable: false, maxLength: 7, unicode: false),
                        gru_data_cricao = c.DateTime(nullable: false, precision: 0),
                        gru_plano = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.gru_id);
            
            CreateTable(
                "dbo.xpg_experiencia_grupo",
                c => new
                    {
                        gru_id = c.Int(nullable: false),
                        tsk_id = c.Int(nullable: false),
                        xpg_valor = c.Int(nullable: false),
                        xpg_data = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.gru_id, t.tsk_id })
                .ForeignKey("dbo.gru_grupo", t => t.gru_id, cascadeDelete: true)
                .ForeignKey("dbo.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .Index(t => t.gru_id)
                .Index(t => t.tsk_id);
            
            CreateTable(
                "dbo.msg_mensagem",
                c => new
                    {
                        msg_id = c.Int(nullable: false, identity: true),
                        usu_id_remetente = c.Int(nullable: false),
                        usu_id_destinatario = c.Int(),
                        gru_id_destinatario = c.Int(),
                        msg_conteudo = c.String(nullable: false, maxLength: 120, unicode: false),
                        msg_data = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.msg_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_destinatario, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_remetente, cascadeDelete: true)
                .ForeignKey("dbo.gru_grupo", t => t.gru_id_destinatario, cascadeDelete: true)
                .Index(t => t.usu_id_remetente)
                .Index(t => t.usu_id_destinatario)
                .Index(t => t.gru_id_destinatario);
            
            CreateTable(
                "dbo.usu_usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        usu_sobrenome = c.String(nullable: false, maxLength: 20, unicode: false),
                        usu_data_nascimento = c.DateTime(nullable: false, storeType: "date"),
                        usu_sexo = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        Email = c.String(unicode: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 0),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.crt_cartao",
                c => new
                    {
                        crt_id = c.Int(nullable: false, identity: true),
                        usu_id = c.Int(nullable: false),
                        crt_bandeira = c.String(nullable: false, unicode: false),
                        crt_numero_cartao = c.String(nullable: false, unicode: false),
                        crt_nome_titular = c.String(nullable: false, unicode: false),
                        crt_data_vencimento = c.String(nullable: false, unicode: false),
                        crt_codigo_seguranca = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.crt_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id);
            
            CreateTable(
                "dbo.CustomUserClaim",
                c => new
                    {
                        CustomUserClaimId = c.Int(nullable: false, identity: true),
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.CustomUserClaimId)
                .ForeignKey("dbo.usu_usuario", t => t.Id, cascadeDelete: true)
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
                .ForeignKey("dbo.usu_usuario", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.xpu_experiencia_usuario",
                c => new
                    {
                        usu_id = c.Int(nullable: false),
                        tsk_id = c.Int(nullable: false),
                        xpg_valor = c.Int(nullable: false),
                        xpu_data = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.usu_id, t.tsk_id })
                .ForeignKey("dbo.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id)
                .Index(t => t.tsk_id);
            
            CreateTable(
                "dbo.feb_feedback",
                c => new
                    {
                        feb_id = c.Int(nullable: false, identity: true),
                        feb_relatorio = c.String(nullable: false, maxLength: 150, unicode: false),
                        feb_nota = c.Int(),
                        feb_feedback = c.String(maxLength: 150, unicode: false),
                        feb_data_conclusao = c.DateTime(nullable: false, precision: 0),
                        tsk_id = c.Int(nullable: false),
                        usu_id_responsavel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.feb_id)
                .ForeignKey("dbo.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_responsavel)
                .Index(t => t.tsk_id)
                .Index(t => t.usu_id_responsavel);
            
            CreateTable(
                "dbo.CustomUserLogin",
                c => new
                    {
                        CustomUserLoginId = c.Int(nullable: false, identity: true),
                        LoginProvider = c.String(unicode: false),
                        ProviderKey = c.String(unicode: false),
                        UserId = c.Int(nullable: false),
                        ApplicationUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.CustomUserLoginId)
                .ForeignKey("dbo.usu_usuario", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.CustomUserRole",
                c => new
                    {
                        CustomUserRoleId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        ApplicationUser_Id = c.Int(),
                        CustomRole_CustomRoleId = c.Int(),
                    })
                .PrimaryKey(t => t.CustomUserRoleId)
                .ForeignKey("dbo.usu_usuario", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.CustomRole", t => t.CustomRole_CustomRoleId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.CustomRole_CustomRoleId);
            
            CreateTable(
                "dbo.tel_telefone",
                c => new
                    {
                        tel_id = c.Int(nullable: false, identity: true),
                        usu_id = c.Int(nullable: false),
                        tel_ddd = c.Int(nullable: false),
                        tel_numero = c.Int(nullable: false),
                        tel_tipo = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.tel_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id);
            
            CreateTable(
                "dbo.uxg_usuario_grupo",
                c => new
                    {
                        usu_id = c.Int(nullable: false),
                        gru_id = c.Int(nullable: false),
                        uxg_administrador = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.usu_id, t.gru_id })
                .ForeignKey("dbo.gru_grupo", t => t.gru_id, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id)
                .Index(t => t.gru_id);
            
            CreateTable(
                "dbo.sem_semana",
                c => new
                    {
                        sem_id = c.Int(nullable: false, identity: true),
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
                "dbo.CustomRole",
                c => new
                    {
                        CustomRoleId = c.Int(nullable: false, identity: true),
                        Id = c.Int(nullable: false),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.CustomRoleId);
            
            CreateTable(
                "dbo.qxs_quest_semana",
                c => new
                    {
                        qst_id = c.Int(nullable: false),
                        sem_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.qst_id, t.sem_id })
                .ForeignKey("dbo.qst_quest", t => t.qst_id, cascadeDelete: true)
                .ForeignKey("dbo.sem_semana", t => t.sem_id, cascadeDelete: true)
                .Index(t => t.qst_id)
                .Index(t => t.sem_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomUserRole", "CustomRole_CustomRoleId", "dbo.CustomRole");
            DropForeignKey("dbo.pre_precedencia", "tsk_id_precedente", "dbo.tsk_task");
            DropForeignKey("dbo.arq_arquivos", "tsk_id", "dbo.tsk_task");
            DropForeignKey("dbo.pre_precedencia", "tsk_id_antecedente", "dbo.tsk_task");
            DropForeignKey("dbo.tsk_task", "qst_id", "dbo.qst_quest");
            DropForeignKey("dbo.qxs_quest_semana", "sem_id", "dbo.sem_semana");
            DropForeignKey("dbo.qxs_quest_semana", "qst_id", "dbo.qst_quest");
            DropForeignKey("dbo.pre_precedencia", "qst_id_precedente", "dbo.qst_quest");
            DropForeignKey("dbo.qst_quest", "gru_id_criador", "dbo.gru_grupo");
            DropForeignKey("dbo.msg_mensagem", "gru_id_destinatario", "dbo.gru_grupo");
            DropForeignKey("dbo.uxg_usuario_grupo", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.uxg_usuario_grupo", "gru_id", "dbo.gru_grupo");
            DropForeignKey("dbo.tel_telefone", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.tsk_task", "usu_id_responsavel", "dbo.usu_usuario");
            DropForeignKey("dbo.CustomUserRole", "ApplicationUser_Id", "dbo.usu_usuario");
            DropForeignKey("dbo.msg_mensagem", "usu_id_remetente", "dbo.usu_usuario");
            DropForeignKey("dbo.qst_quest", "usu_id_criador", "dbo.usu_usuario");
            DropForeignKey("dbo.CustomUserLogin", "ApplicationUser_Id", "dbo.usu_usuario");
            DropForeignKey("dbo.feb_feedback", "usu_id_responsavel", "dbo.usu_usuario");
            DropForeignKey("dbo.feb_feedback", "tsk_id", "dbo.tsk_task");
            DropForeignKey("dbo.xpu_experiencia_usuario", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.xpu_experiencia_usuario", "tsk_id", "dbo.tsk_task");
            DropForeignKey("dbo.msg_mensagem", "usu_id_destinatario", "dbo.usu_usuario");
            DropForeignKey("dbo.AspNetClients", "ApplicationUser_Id", "dbo.usu_usuario");
            DropForeignKey("dbo.CustomUserClaim", "Id", "dbo.usu_usuario");
            DropForeignKey("dbo.crt_cartao", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.xpg_experiencia_grupo", "tsk_id", "dbo.tsk_task");
            DropForeignKey("dbo.xpg_experiencia_grupo", "gru_id", "dbo.gru_grupo");
            DropForeignKey("dbo.pre_precedencia", "qst_id_antecedente", "dbo.qst_quest");
            DropIndex("dbo.qxs_quest_semana", new[] { "sem_id" });
            DropIndex("dbo.qxs_quest_semana", new[] { "qst_id" });
            DropIndex("dbo.uxg_usuario_grupo", new[] { "gru_id" });
            DropIndex("dbo.uxg_usuario_grupo", new[] { "usu_id" });
            DropIndex("dbo.tel_telefone", new[] { "usu_id" });
            DropIndex("dbo.CustomUserRole", new[] { "CustomRole_CustomRoleId" });
            DropIndex("dbo.CustomUserRole", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CustomUserLogin", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.feb_feedback", new[] { "usu_id_responsavel" });
            DropIndex("dbo.feb_feedback", new[] { "tsk_id" });
            DropIndex("dbo.xpu_experiencia_usuario", new[] { "tsk_id" });
            DropIndex("dbo.xpu_experiencia_usuario", new[] { "usu_id" });
            DropIndex("dbo.AspNetClients", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CustomUserClaim", new[] { "Id" });
            DropIndex("dbo.crt_cartao", new[] { "usu_id" });
            DropIndex("dbo.msg_mensagem", new[] { "gru_id_destinatario" });
            DropIndex("dbo.msg_mensagem", new[] { "usu_id_destinatario" });
            DropIndex("dbo.msg_mensagem", new[] { "usu_id_remetente" });
            DropIndex("dbo.xpg_experiencia_grupo", new[] { "tsk_id" });
            DropIndex("dbo.xpg_experiencia_grupo", new[] { "gru_id" });
            DropIndex("dbo.qst_quest", new[] { "gru_id_criador" });
            DropIndex("dbo.qst_quest", new[] { "usu_id_criador" });
            DropIndex("dbo.pre_precedencia", new[] { "tsk_id_precedente" });
            DropIndex("dbo.pre_precedencia", new[] { "qst_id_precedente" });
            DropIndex("dbo.pre_precedencia", new[] { "tsk_id_antecedente" });
            DropIndex("dbo.pre_precedencia", new[] { "qst_id_antecedente" });
            DropIndex("dbo.tsk_task", new[] { "usu_id_responsavel" });
            DropIndex("dbo.tsk_task", new[] { "qst_id" });
            DropIndex("dbo.arq_arquivos", new[] { "tsk_id" });
            DropTable("dbo.qxs_quest_semana");
            DropTable("dbo.CustomRole");
            DropTable("dbo.AspNetClaims");
            DropTable("dbo.sem_semana");
            DropTable("dbo.uxg_usuario_grupo");
            DropTable("dbo.tel_telefone");
            DropTable("dbo.CustomUserRole");
            DropTable("dbo.CustomUserLogin");
            DropTable("dbo.feb_feedback");
            DropTable("dbo.xpu_experiencia_usuario");
            DropTable("dbo.AspNetClients");
            DropTable("dbo.CustomUserClaim");
            DropTable("dbo.crt_cartao");
            DropTable("dbo.usu_usuario");
            DropTable("dbo.msg_mensagem");
            DropTable("dbo.xpg_experiencia_grupo");
            DropTable("dbo.gru_grupo");
            DropTable("dbo.qst_quest");
            DropTable("dbo.pre_precedencia");
            DropTable("dbo.tsk_task");
            DropTable("dbo.arq_arquivos");
        }
    }
}
