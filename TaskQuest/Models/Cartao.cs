using System.ComponentModel.DataAnnotations;

namespace TaskQuest.Models
{
    public class Cartao
    {

        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Bandeira { get; set; }

        public string Numero { get; set; }

        public string NomeTitular { get; set; }

        public string DataVencimento { get; set; }

        public string CodigoSeguranca { get; set; }

        public string Senha { get; set; }

        public virtual User Usuario { get; set; }
    }
}