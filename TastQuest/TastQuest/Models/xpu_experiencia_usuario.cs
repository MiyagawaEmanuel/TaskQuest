namespace TastQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("taskquest.xpu_experiencia_usuario")]
    public partial class xpu_experiencia_usuario
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int usu_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int tsk_id { get; set; }

        public int xpu_valor { get; set; }

        public DateTime xpu_data { get; set; }

        public virtual tsk_task tsk_task { get; set; }

        public virtual usu_usuario usu_usuario { get; set; }
    }
}
