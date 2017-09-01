namespace TaskQuest.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string ClientKey { get; set; }

        public int? UsuarioId { get; set; }

        public virtual User Usuario { get; set; }
        
    }
}