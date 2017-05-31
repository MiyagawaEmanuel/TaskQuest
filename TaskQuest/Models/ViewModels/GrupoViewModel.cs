using System.Collections.Generic;


namespace TaskQuest.ViewModels
{
    public class GrupoViewModel
    {
        public string Nome { get; set; }

        public string Cor { get; set; }
        
        public List<IntegranteViewModel> Integrantes = new List<IntegranteViewModel>();
    }

    public class IntegranteViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Cor { get; set; }
    }
}