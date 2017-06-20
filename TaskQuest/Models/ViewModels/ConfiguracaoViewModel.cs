using System.Collections.Generic;
using TaskQuest.Models;

namespace TaskQuest.ViewModels
{
    public class ConfiguracaoViewModel
    {
        public User usuario = new User();

        public List<Cartao> Cartoes = new List<Cartao>();

        public List<Telefone> Telefones = new List<Telefone>();

        public Telefone Telefone = new Telefone();

        public Cartao Cartao = new Cartao();

    }
}
