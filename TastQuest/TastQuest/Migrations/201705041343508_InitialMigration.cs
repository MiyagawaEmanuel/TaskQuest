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
                    arq_id = c.Int(nullable: false, identity: true),
                    arq_nome = c.String(nullable: false, maxLength: 20, unicode: false),
                    arq_caminho = c.String(nullable: false, maxLength: 40, unicode: false),
                    arq_tamanho = c.Int(nullable: false),
                    arq_data_upload = c.DateTime(nullable: false, precision: 0, defaultValue: DateTime.Now),
                    tsk_id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.arq_id)
                .ForeignKey("taskquest.tsk_task", t => t.tsk_id, cascadeDelete: true)
                .Index(t => t.tsk_id);

            CreateTable(
                "taskquest.tsk_task",
                c => new
                {
                    tsk_id = c.Int(nullable: false, identity: true),
                    qst_id = c.Int(nullable: false),
                    tsk_nome = c.String(nullable: false, maxLength: 45, unicode: false),
                    tsk_status = c.Int(nullable: false),
                    tsk_dificuldade = c.Int(nullable: false),
                    tsk_duracao = c.Time(precision: 2),
                    tsk_data_criacao = c.DateTime(nullable: false, precision: 0, defaultValue: DateTime.Now),
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
                    feb_id = c.Int(nullable: false, identity: true),
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
                    qst_id = c.Int(nullable: false, identity: true),
                    usu_id_criador = c.Int(),
                    gru_id_criador = c.Int(),
                    qst_cor = c.String(nullable: false, maxLength: 45, unicode: false),
                    qst_criacao = c.DateTime(nullable: false, precision: 0, defaultValue: DateTime.Now),
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
                    gru_id = c.Int(nullable: false, identity: true),
                    gru_nome = c.String(nullable: false, maxLength: 20, unicode: false),
                    gru_cor = c.String(nullable: false, maxLength: 7, unicode: false),
                    gru_data_criacao = c.DateTime(nullable: false, precision: 0, defaultValue: DateTime.Now),
                    gru_plano = c.Boolean(nullable: false, defaultValue: false),
                })
                .PrimaryKey(t => t.gru_id);

            CreateTable(
                "taskquest.msg_mensagem",
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
                    TwoFactorEnabled = c.Boolean(nullable: false),
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
                    usu_id = c.Int(nullable: false),
                    crt_bandeira = c.String(nullable: false),
                    crt_numero_cartao = c.String(nullable: false),
                    crt_nome_titular = c.String(nullable: false),
                    crt_data_vencimento = c.String(nullable: false),
                    crt_codigo_seguranca = c.String(nullable: false),
                })
                .PrimaryKey(t => t.crt_id)
                .ForeignKey("taskquest.usu_usuario", t => t.usu_id, cascadeDelete: true)
                .Index(t => t.usu_id);

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
                    uxg_administrador = c.Boolean(nullable: false, defaultValue: false),
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
                    xpu_data = c.DateTime(nullable: false, precision: 0, defaultValue: DateTime.Now),
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
                    xpg_valor = c.Int(nullable: false),
                    xpg_data = c.DateTime(nullable: false, precision: 0, defaultValue: DateTime.Now),
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
    }
}