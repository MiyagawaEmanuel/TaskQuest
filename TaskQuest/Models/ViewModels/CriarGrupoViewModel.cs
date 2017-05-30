using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.ViewModels
{
    public class CriarGrupoViewModel
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Cor { get; set; }

        public DateTime DataCriacao { get; set; }

        public bool Plano { get; set; }
        
    }
}