using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.ViewModels
{
    public class UsuarioViewModel
    {
        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string Email { get; set; }

        //YYYY-MM-DD
        public string DataNascimento { get; set; }
        
        public string Senha { get; set; }

        //public string Cor { get; set; }

        public string Sexo { get; set; }

        public List<CartaoViewModel> Cartoes = new List<CartaoViewModel>();

        public List<TelefoneViewModel> Telefones = new List<TelefoneViewModel>();

        public TelefoneViewModel telefoneViewModel = new TelefoneViewModel();

        public CartaoViewModel cartaoViewModel = new CartaoViewModel();

    }

    public class CartaoViewModel
    {
        public int Id { get; set; }

        public string Bandeira { get; set; }

        public string Numero { get; set; }

        public string NomeTitular { get; set; }

        public string DataVencimento { get; set; }

        public string CodigoSeguranca { get; set; }
    }

    public class TelefoneViewModel
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public int Ddd { get; set; }

        public int Numero { get; set; }
    }
}