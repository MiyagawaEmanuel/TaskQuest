namespace TaskQuest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.bkp_backup",
                c => new
                    {
                        bkp_id = c.Int(nullable: false, identity: true),
                        bkp_table_name = c.String(nullable: false, unicode: false),
                        bkp_query_type = c.String(nullable: false, unicode: false),
                        bkp_data = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.bkp_id);
            
            CreateTable(
                "dbo.cli_client",
                c => new
                    {
                        cli_id = c.Int(nullable: false, identity: true),
                        cli_key = c.String(unicode: false),
                        usu_id = c.Int(),
                    })
                .PrimaryKey(t => t.cli_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id);
            
            CreateTable(
                "dbo.usu_usuario",
                c => new
                    {
                        usu_id = c.Int(nullable: false, identity: true),
                        usu_nome = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        usu_sobrenome = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        usu_data_nascimento = c.DateTime(nullable: false, precision: 0),
                        usu_sexo = c.String(nullable: false, maxLength: 1, storeType: "nvarchar"),
                        usu_cor = c.String(nullable: false, maxLength: 7, storeType: "nvarchar"),
                        usu_email = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        usu_email_confirmado = c.Boolean(nullable: false),
                        usu_senha = c.String(unicode: false),
                        usu_selo_seguranca = c.String(unicode: false),
                        usu_dois_passos_login = c.Boolean(nullable: false),
                        usu_data_desbloqueio = c.DateTime(precision: 0),
                        usu_bloqueado = c.Boolean(nullable: false),
                        usu_contador_acesso_falho = c.Int(nullable: false),
                        usu_user_name = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.usu_id)
                .Index(t => t.usu_email, unique: true, name: "usu_email_unique_idx")
                .Index(t => t.usu_user_name, unique: true, name: "usu_user_name_unique_idx");
            
            CreateTable(
                "dbo.cuc_custom_user_claim",
                c => new
                    {
                        cuc_id = c.Int(nullable: false, identity: true),
                        usu_id = c.Int(nullable: false),
                        cuc_type = c.String(unicode: false),
                        cuc_value = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.cuc_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id);
            
            CreateTable(
                "dbo.msg_mensagem",
                c => new
                    {
                        msg_id = c.Int(nullable: false, identity: true),
                        usu_id_remetente = c.Int(nullable: false),
                        usu_id_destinatario = c.Int(),
                        gru_id_destinatario = c.Int(),
                        msg_conteudo = c.String(nullable: false, maxLength: 120, storeType: "nvarchar"),
                        msg_data = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.msg_id)
                .ForeignKey("dbo.gru_grupo", t => t.gru_id_destinatario, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_destinatario, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_remetente, cascadeDelete: true)
                .Index(t => t.usu_id_remetente)
                .Index(t => t.usu_id_destinatario)
                .Index(t => t.gru_id_destinatario);
            
            CreateTable(
                "dbo.gru_grupo",
                c => new
                    {
                        gru_id = c.Int(nullable: false, identity: true),
                        gru_nome = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        gru_cor = c.String(nullable: false, maxLength: 7, storeType: "nvarchar"),
                        gru_data_criacao = c.DateTime(nullable: false, precision: 0),
                        gru_descricao = c.String(nullable: false, maxLength: 120, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.gru_id);
            
            CreateTable(
                "dbo.not_notificacao",
                c => new
                    {
                        not_id = c.Int(nullable: false, identity: true),
                        not_texto = c.String(nullable: false, unicode: false),
                        not_tipo_notificacao = c.String(nullable: false, unicode: false),
                        not_entidade_modificada = c.String(nullable: false, unicode: false),
                        not_data_notificacao = c.DateTime(nullable: false, precision: 0),
                        gru_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.not_id)
                .ForeignKey("dbo.gru_grupo", t => t.gru_id, cascadeDelete: true)
                .Index(t => t.gru_id);
            
            CreateTable(
                "dbo.pag_pagamento",
                c => new
                    {
                        pag_id = c.Int(nullable: false, identity: true),
                        pag_code = c.String(nullable: false, unicode: false),
                        pag_satus = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.pag_id)
                .ForeignKey("dbo.gru_grupo", t => t.pag_id)
                .Index(t => t.pag_id);
            
            CreateTable(
                "dbo.qst_quest",
                c => new
                    {
                        qst_id = c.Int(nullable: false, identity: true),
                        usu_id_criador = c.Int(),
                        gru_id_criador = c.Int(),
                        qst_cor = c.String(nullable: false, maxLength: 7, storeType: "nvarchar"),
                        qst_data_criacao = c.DateTime(nullable: false, precision: 0),
                        qst_descricao = c.String(nullable: false, maxLength: 300, storeType: "nvarchar"),
                        qst_nome = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.qst_id)
                .ForeignKey("dbo.gru_grupo", t => t.gru_id_criador, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_criador, cascadeDelete: true)
                .Index(t => t.usu_id_criador)
                .Index(t => t.gru_id_criador);
            
            CreateTable(
                "dbo.tsk_task",
                c => new
                    {
                        tsk_id = c.Int(nullable: false, identity: true),
                        qst_id = c.Int(nullable: false),
                        tsk_nome = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        tsk_descricao = c.String(nullable: false, maxLength: 300, storeType: "nvarchar"),
                        tsk_status = c.Int(nullable: false),
                        tsk_dificuldade = c.Int(nullable: false),
                        tsk_data_criacao = c.DateTime(nullable: false, precision: 0),
                        tsk_data_inicio = c.DateTime(nullable: false, precision: 0),
                        tsk_data_conclusao = c.DateTime(nullable: false, precision: 0),
                        tsk_verificacao = c.Boolean(nullable: false),
                        usu_id_responsavel = c.Int(),
                    })
                .PrimaryKey(t => t.tsk_id)
                .ForeignKey("dbo.qst_quest", t => t.qst_id, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_responsavel, cascadeDelete: true)
                .Index(t => t.qst_id)
                .Index(t => t.usu_id_responsavel);
            
            CreateTable(
                "dbo.feb_feedback",
                c => new
                    {
                        feb_id = c.Int(nullable: false, identity: true),
                        feb_relatorio = c.String(maxLength: 300, storeType: "nvarchar"),
                        feb_nota = c.Int(nullable: false),
                        feb_resposta = c.String(maxLength: 300, storeType: "nvarchar"),
                        feb_data_criacao = c.DateTime(nullable: false, precision: 0),
                        tsk_id = c.Int(nullable: false),
                        usu_id_responsavel = c.Int(),
                    })
                .PrimaryKey(t => t.feb_id)
                .ForeignKey("dbo.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id_responsavel, cascadeDelete: true)
                .Index(t => t.tsk_id)
                .Index(t => t.usu_id_responsavel);
            
            CreateTable(
                "dbo.fle_file",
                c => new
                    {
                        fle_id = c.Int(nullable: false, identity: true),
                        fle_nome = c.String(nullable: false, maxLength: 120, storeType: "nvarchar"),
                        fle_url = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        fle_response = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        fle_size = c.Double(nullable: false),
                        fle_data_envio = c.DateTime(nullable: false, precision: 0),
                        fle_validado = c.Boolean(nullable: false),
                        tsk_id = c.Int(),
                        usu_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.fle_id)
                .ForeignKey("dbo.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.tsk_id)
                .Index(t => t.usu_id);
            
            CreateTable(
                "dbo.ptu_ponto_usuario",
                c => new
                    {
                        ptu_id = c.Int(nullable: false),
                        usu_id = c.Int(nullable: false),
                        ptu_valor = c.Int(nullable: false),
                        ptu_data = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.ptu_id, t.usu_id })
                .ForeignKey("dbo.tsk_task", t => t.ptu_id, cascadeDelete: true)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.ptu_id)
                .Index(t => t.usu_id);
            
            CreateTable(
                "dbo.cul_custom_user_login",
                c => new
                    {
                        cul_id = c.Int(nullable: false, identity: true),
                        cul_login_provider = c.String(unicode: false),
                        cul_provider_key = c.String(unicode: false),
                        usu_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.cul_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id);
            
            CreateTable(
                "dbo.cur_custom_user_role",
                c => new
                    {
                        cur_id = c.Int(nullable: false, identity: true),
                        usu_id = c.Int(nullable: false),
                        rol_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.cur_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .ForeignKey("dbo.ctr_custom_role", t => t.rol_id, cascadeDelete: true)
                .Index(t => t.usu_id)
                .Index(t => t.rol_id);
            
            CreateTable(
                "dbo.tel_telefone",
                c => new
                    {
                        tel_id = c.Int(nullable: false, identity: true),
                        usu_id = c.Int(nullable: false),
                        tel_numero = c.String(nullable: false, unicode: false),
                        tel_tipo = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.tel_id)
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id);
            
            CreateTable(
                "dbo.ctr_custom_role",
                c => new
                    {
                        ctr_Id = c.Int(nullable: false, identity: true),
                        ctr_name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ctr_Id);
            
            CreateTable(
                "dbo.uxg_usuario_grupo",
                c => new
                    {
                        usu_id = c.Int(nullable: false),
                        gru_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.usu_id, t.gru_id })
                .ForeignKey("dbo.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .ForeignKey("dbo.gru_grupo", t => t.gru_id, cascadeDelete: true)
                .Index(t => t.usu_id)
                .Index(t => t.gru_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.cur_custom_user_role", "rol_id", "dbo.ctr_custom_role");
            DropForeignKey("dbo.tel_telefone", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.tsk_task", "usu_id_responsavel", "dbo.usu_usuario");
            DropForeignKey("dbo.cur_custom_user_role", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.msg_mensagem", "usu_id_remetente", "dbo.usu_usuario");
            DropForeignKey("dbo.qst_quest", "usu_id_criador", "dbo.usu_usuario");
            DropForeignKey("dbo.cul_custom_user_login", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.uxg_usuario_grupo", "gru_id", "dbo.gru_grupo");
            DropForeignKey("dbo.uxg_usuario_grupo", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.fle_file", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.feb_feedback", "usu_id_responsavel", "dbo.usu_usuario");
            DropForeignKey("dbo.msg_mensagem", "usu_id_destinatario", "dbo.usu_usuario");
            DropForeignKey("dbo.qst_quest", "gru_id_criador", "dbo.gru_grupo");
            DropForeignKey("dbo.tsk_task", "qst_id", "dbo.qst_quest");
            DropForeignKey("dbo.ptu_ponto_usuario", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.ptu_ponto_usuario", "ptu_id", "dbo.tsk_task");
            DropForeignKey("dbo.fle_file", "tsk_id", "dbo.tsk_task");
            DropForeignKey("dbo.feb_feedback", "tsk_id", "dbo.tsk_task");
            DropForeignKey("dbo.pag_pagamento", "pag_id", "dbo.gru_grupo");
            DropForeignKey("dbo.not_notificacao", "gru_id", "dbo.gru_grupo");
            DropForeignKey("dbo.msg_mensagem", "gru_id_destinatario", "dbo.gru_grupo");
            DropForeignKey("dbo.cli_client", "usu_id", "dbo.usu_usuario");
            DropForeignKey("dbo.cuc_custom_user_claim", "usu_id", "dbo.usu_usuario");
            DropIndex("dbo.uxg_usuario_grupo", new[] { "gru_id" });
            DropIndex("dbo.uxg_usuario_grupo", new[] { "usu_id" });
            DropIndex("dbo.tel_telefone", new[] { "usu_id" });
            DropIndex("dbo.cur_custom_user_role", new[] { "rol_id" });
            DropIndex("dbo.cur_custom_user_role", new[] { "usu_id" });
            DropIndex("dbo.cul_custom_user_login", new[] { "usu_id" });
            DropIndex("dbo.ptu_ponto_usuario", new[] { "usu_id" });
            DropIndex("dbo.ptu_ponto_usuario", new[] { "ptu_id" });
            DropIndex("dbo.fle_file", new[] { "usu_id" });
            DropIndex("dbo.fle_file", new[] { "tsk_id" });
            DropIndex("dbo.feb_feedback", new[] { "usu_id_responsavel" });
            DropIndex("dbo.feb_feedback", new[] { "tsk_id" });
            DropIndex("dbo.tsk_task", new[] { "usu_id_responsavel" });
            DropIndex("dbo.tsk_task", new[] { "qst_id" });
            DropIndex("dbo.qst_quest", new[] { "gru_id_criador" });
            DropIndex("dbo.qst_quest", new[] { "usu_id_criador" });
            DropIndex("dbo.pag_pagamento", new[] { "pag_id" });
            DropIndex("dbo.not_notificacao", new[] { "gru_id" });
            DropIndex("dbo.msg_mensagem", new[] { "gru_id_destinatario" });
            DropIndex("dbo.msg_mensagem", new[] { "usu_id_destinatario" });
            DropIndex("dbo.msg_mensagem", new[] { "usu_id_remetente" });
            DropIndex("dbo.cuc_custom_user_claim", new[] { "usu_id" });
            DropIndex("dbo.usu_usuario", "usu_user_name_unique_idx");
            DropIndex("dbo.usu_usuario", "usu_email_unique_idx");
            DropIndex("dbo.cli_client", new[] { "usu_id" });
            DropTable("dbo.uxg_usuario_grupo");
            DropTable("dbo.ctr_custom_role");
            DropTable("dbo.tel_telefone");
            DropTable("dbo.cur_custom_user_role");
            DropTable("dbo.cul_custom_user_login");
            DropTable("dbo.ptu_ponto_usuario");
            DropTable("dbo.fle_file");
            DropTable("dbo.feb_feedback");
            DropTable("dbo.tsk_task");
            DropTable("dbo.qst_quest");
            DropTable("dbo.pag_pagamento");
            DropTable("dbo.not_notificacao");
            DropTable("dbo.gru_grupo");
            DropTable("dbo.msg_mensagem");
            DropTable("dbo.cuc_custom_user_claim");
            DropTable("dbo.usu_usuario");
            DropTable("dbo.cli_client");
            DropTable("dbo.bkp_backup");
        }
    }
}
