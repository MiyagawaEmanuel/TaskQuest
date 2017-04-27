namespace TastQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("taskquest.pre_precedencia")]
    public partial class pre_precedencia
    {
        [Key]
        public int pre_id { get; set; }

        public int? qst_id_antecedente { get; set; }

        public int? tsk_id_antecedente { get; set; }

        public int? qst_id_precedente { get; set; }

        public int? tsk_id_precedente { get; set; }

        public int usu_id_responsavel_conclusao { get; set; }

        public virtual qst_quest qst_quest { get; set; }

        public virtual qst_quest qst_quest1 { get; set; }

        public virtual tsk_task tsk_task { get; set; }

        public virtual tsk_task tsk_task1 { get; set; }

        public virtual usu_usuario usu_usuario { get; set; }
    }
}
