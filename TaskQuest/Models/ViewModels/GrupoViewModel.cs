using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.ViewModels
{
    public class GrupoViewModel
    {
        public string Nome { get; set; }

        public string Cor { get; set; }
        
        public List<IntegranteViewModel> integrantes = new List<IntegranteViewModel>();
    }

    public class IntegranteViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Cor { get; set; }
    }
}