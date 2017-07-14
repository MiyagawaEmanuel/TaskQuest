using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.ViewModels
{
    public class ConfiguracaoViewModel
    {
        public UserViewModel usuario = new UserViewModel();

        public List<Cartao> Cartoes = new List<Cartao>();

        public List<Telefone> Telefones = new List<Telefone>();

        public Telefone Telefone = new Telefone();

        public Cartao Cartao = new Cartao();

    }

    public class AdicionarIntegranteViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string GrupoId { get; set; }
    }

    public class UserViewModel
    {

        public UserViewModel() { }

        public UserViewModel(User user)
        {
            Nome = user.Nome;
            Sobrenome = user.Sobrenome;
            DataNascimento = user.DataNascimento.ToString("yyyy-MM-dd");
            Email = user.Email;
            Senha = user.PasswordHash;
            ConfirmarSenha = user.PasswordHash;
            Cor = user.Cor;
        }

        public User Update()
        {
            using (var db = new DbContext())
            {
                var aux = db.Users.ToList().Where(q => Util.Hash(q.Id.ToString()) == this.Id);
                if (aux.Any())
                {

                    User user = aux.First();

                    user.Nome = Nome;
                    user.Sobrenome = Sobrenome;
                    //user.Sexo = Sexo;
                    user.DataNascimento = DataNascimento.StringToDateTime();
                    user.Email = Email;
                    user.PasswordHash = Senha;
                    user.Cor = Cor;

                    return user;
                }
                else
                    return null;
            }
        }

        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Sobrenome { get; set; }

        [Date]
        [Required]
        public string DataNascimento { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        [Compare("Senha")]
        public string ConfirmarSenha { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 4)]
        public string Cor { get; set; }

    }

}
