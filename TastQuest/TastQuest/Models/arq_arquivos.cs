using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastQuest.Models
{
    [Table("taskquest.arq_arquivos")]
    public partial class arq_arquivos
    {
        
        [Key]
        public int arq_id { get; set; }

        [Required]
        [StringLength(20)]
        public string arq_nome { get; set; }

        [Required]
        [StringLength(40)]
        public string arq_caminho { get; set; }
        
        [Required]
        public int arq_tamanho { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime arq_data_upload { get; set; }

        [Required]
        public virtual tsk_task tsk_task { get; set; }

    }
}
