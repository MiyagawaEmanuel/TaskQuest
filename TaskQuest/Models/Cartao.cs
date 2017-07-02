using System.ComponentModel.DataAnnotations;

namespace TaskQuest.Models
{
    public class Cartao
    {

        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [Required]
        public string Bandeira { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Numero { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string NomeTitular { get; set; }

        [Required]
        [StringLength(10)]
        public string DataVencimento { get; set; }

        [Required]
        [StringLength(3)]
        public string CodigoSeguranca { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Senha { get; set; }

        public virtual User Usuario { get; set; }
    }
}