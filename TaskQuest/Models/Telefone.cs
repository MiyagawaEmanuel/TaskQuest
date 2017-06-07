namespace TaskQuest.Models
{
    public class Telefone
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int Ddd { get; set; }

        public int Numero { get; set; }

        public string Tipo { get; set; }

        public virtual User Usuario { get; set; }
    }
}