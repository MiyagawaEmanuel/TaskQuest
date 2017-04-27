namespace TastQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("taskquest.uxg_usuario_grupo")]
    public partial class uxg_usuario_grupo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int usu_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int gru_id { get; set; }

        public bool uxg_administrador { get; set; }

        public virtual gru_grupo gru_grupo { get; set; }

        public virtual usu_usuario usu_usuario { get; set; }
    }
}
