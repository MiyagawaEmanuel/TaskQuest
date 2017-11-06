using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskQuest.Data;
using TaskQuest.Models;

namespace TaskQuest.ViewModels
{
    public class ConfiguracaoViewModel
    {

        public UserViewModel usuario = new UserViewModel();

        public List<TelefoneViewModel> Telefones = new List<TelefoneViewModel>();

    }

    public class UserViewModel
    {

        public UserViewModel() { }

        public UserViewModel(User user)
        {
            this.Id = Util.Encrypt(user.Id.ToString());
            this.Nome = user.Nome;
            this.Sobrenome = user.Sobrenome;
            this.DataNascimento = user.DataNascimento.ToString("yyyy-MM-dd");
            this.Cor = user.Cor;
        }

        public User Update()
        {
            using (var db = new DbContext())
            {
                int Id;
                if (int.TryParse(Util.Decrypt(this.Id), out Id))
                {

                    User user = db.Users.Find(Id);

                    user.Nome = Nome;
                    user.Sobrenome = Sobrenome;
                    user.DataNascimento = DataNascimento.StringToDateTime();
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
        [StringLength(7, MinimumLength = 4)]
        public string Cor { get; set; }

    }

    public class TelefoneViewModel
    {
        public TelefoneViewModel() { }

        public TelefoneViewModel(Telefone telefone)
        {
            this.Id = Util.Encrypt(telefone.Id.ToString());
            this.Tipo = telefone.Tipo;
            this.Numero = telefone.Numero;
        }

        public Telefone Update()
        {
            if (string.IsNullOrEmpty(this.Id))
            {
                Telefone telefone = new Telefone()
                {
                    Tipo = this.Tipo,
                    Numero = this.Numero,
                };
                
                return telefone;
            }
            else
            {
                using (var db = new DbContext())
                {
                    int Id;
                    if (int.TryParse(Util.Decrypt(this.Id), out Id))
                    {
                        Telefone telefone = db.Telefone.Find(Id);
                        telefone.Tipo = this.Tipo;
                        telefone.Numero = this.Numero;

                        return telefone;
                    }
                    else
                        return null;
                }
            }
        }

        public string Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Tipo { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 14)]
        public string Numero { get; set; }

    }

    public class AlterarSenhaViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string SenhaAtual { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Senha { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Senha")]
        public string ConfirmarSenha { get; set; }
    }

}
