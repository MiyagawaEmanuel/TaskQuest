using System.ComponentModel.DataAnnotations;

namespace TaskQuest.Models
{
    public class Telefone
    {
    	public int Id { get; set; }

        public int UsuarioId { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 14)]
        public string Numero { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Tipo { get; set; }

        public virtual User Usuario { get; set; }
    }
}