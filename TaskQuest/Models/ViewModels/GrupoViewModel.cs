using System.Collections.Generic;

namespace TaskQuest.ViewModels
{
    public class GrupoViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        //public string Cor { get; set; }

        public string Descricao { get; set; }

        public bool Plano { get; set; }
        
        public List<IntegranteViewModel> Integrantes = new List<IntegranteViewModel>();

        public AdicionarUsuarioGrupoViewModel adicionarUsuarioGrupoViewModel = new AdicionarUsuarioGrupoViewModel();

    }

    public class IntegranteViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        //public string Cor { get; set; }
    }
}