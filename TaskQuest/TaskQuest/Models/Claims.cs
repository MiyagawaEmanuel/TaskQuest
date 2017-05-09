using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskQuest.Models
{
    [Table("AspNetClaims")]
    public class Claims
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Fornceca um nome para a Claim")]
        [MaxLength(128, ErrorMessage = "Tamanho maximo {0} excedido")]
        [Display(Name = "Nome da Claim")]
        public string Name { get; set; }
    }
}