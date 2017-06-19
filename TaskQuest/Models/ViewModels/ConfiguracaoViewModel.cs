using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.ViewModels
{
    public class UsuarioViewModel
    {
        User usuario = new User();

        public List<Cartao> Cartoes = new List<Cartao>();

        public List<Telefone> Telefones = new List<Telefone>();

        public TelefoneViewModel telefoneViewModel = new TelefoneViewModel();

        public CartaoViewModel cartaoViewModel = new CartaoViewModel();

    }
}
