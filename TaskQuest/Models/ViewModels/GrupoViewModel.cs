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
        
        public List<Integrante> integrantes = new List<Integrante>();
    }

    public class Integrante
    {
        public string Nome { get; set; }

        public string Cor { get; set; }
    }
}