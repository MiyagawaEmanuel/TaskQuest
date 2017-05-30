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

        public DateTime DataNascimento { get; set; }
        
        public string Senha { get; set; }

        public string Cor { get; set; }

        public char Sexo { get; set; }

    }

    public class Cartao
    {
        public string Bandeira { get; set; }

        public string Numero { get; set; }

        public string NomeTitular { get; set; }

        public string DataVencimento { get; set; }

        public string CodigoSeguranca { get; set; }
    }

    public class Telefone
    {
        public string Tipo { get; set; }

        public string Numero { get; set; }
    }
}