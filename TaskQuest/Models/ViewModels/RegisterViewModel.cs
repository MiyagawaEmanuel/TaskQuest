using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.ViewModels
{
    public class RegisterViewModel
    {

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Telefone { get; set; }

        public string Senha { get; set; }

        public string ConfirmarSenha { get; set; }

        public string Cor { get; set; }

        public char Sexo { get; set; }

    }
}