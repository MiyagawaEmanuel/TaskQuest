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

        public List<CartaoViewModel> Cartoes = new List<CartaoViewModel>();

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
            this.Email = user.Email;
            this.Senha = user.PasswordHash;
            this.ConfirmarSenha = user.PasswordHash;
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

    public class CartaoViewModel
    {
        public CartaoViewModel() { }

        public CartaoViewModel(Cartao cartao)
        {
            this.Id = Util.Encrypt(cartao.Id.ToString());
            this.NomeTitular = cartao.NomeTitular;
            this.Bandeira = cartao.Bandeira;
            this.Numero = cartao.Numero;
            this.Senha = cartao.Senha;
            this.CodigoSeguranca = cartao.CodigoSeguranca;
            this.DataVencimento = cartao.DataVencimento;
        }

        public Cartao Update()
        {
            if (string.IsNullOrEmpty(this.Id))
            {
                Cartao cartao = new Cartao()
                {
                    NomeTitular = this.NomeTitular,
                    Bandeira = this.Bandeira,
                    Numero = this.Numero,
                    Senha = this.Senha,
                    CodigoSeguranca = this.CodigoSeguranca,
                    DataVencimento = this.DataVencimento,
                };

                return cartao;
            }
            else
            {
                using (var db = new DbContext())
                {
                    int Id;
                    if (int.TryParse(Util.Decrypt(this.Id), out Id))
                    {
                        Cartao cartao = db.Cartao.Find(Id);

                        cartao.NomeTitular = this.NomeTitular;
                        cartao.Bandeira = this.Bandeira;
                        cartao.Numero = this.Numero;
                        cartao.Senha = this.Senha;
                        cartao.CodigoSeguranca = this.CodigoSeguranca;
                        cartao.DataVencimento = this.DataVencimento;

                        return cartao;
                    }
                    else
                        return null;
                }
            }
        }

        public string Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string NomeTitular { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Bandeira { get; set; }

        [Required]
        [StringLength(19, MinimumLength = 19)]
        public string Numero { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Senha { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string CodigoSeguranca { get; set; }

        [Date]
        [Required]
        public string DataVencimento { get; set; }

    }

}
