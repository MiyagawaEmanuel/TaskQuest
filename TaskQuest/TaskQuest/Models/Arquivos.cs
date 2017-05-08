using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskQuest.Models
{
    [Table("arq_arquivos")]
    public class Arquivo
    {
        
        [Key]
        [Column("arq_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Column("arq_nome")]
        public string Nome { get; set; }

        [Required]
        [StringLength(40)]
        [Column("arq_caminho")]
        public string Path { get; set; }
        
        [Column("arq_tamanho")]
        public int Size { get; set; }
        
        [Column("arq_data_upload")]
        public DateTime UploadDate { get; set; }

        [Column("tsk_id")]
        public int TaskId { get; set; }

        public virtual Task Task { get; set; }

    }
}