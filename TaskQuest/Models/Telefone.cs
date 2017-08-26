using System.ComponentModel.DataAnnotations;

namespace TaskQuest.Models
{
    public class Telefone
    {
    	public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Numero { get; set; }

        public string Tipo { get; set; }

        public virtual User Usuario { get; set; }
    }
}