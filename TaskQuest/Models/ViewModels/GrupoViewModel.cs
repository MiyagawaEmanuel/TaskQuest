using System.Collections.Generic;

namespace TaskQuest.ViewModels
{
    public class GrupoViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }
        
        public string Descricao { get; set; }

        public bool Plano { get; set; }
        
        public List<IntegranteViewModel> Integrantes = new List<IntegranteViewModel>();

        public AdicionarUsuarioGrupoViewModel adicionarUsuarioGrupoViewModel = new AdicionarUsuarioGrupoViewModel();

        public ExcluirUsuarioGrupo criarExcluirUsuarioGrupo(int usuario_id)
        {
            return new ExcluirUsuarioGrupo()
            {
                gru_id = Id,
                usu_id = usuario_id,
            };
        }

    }

    public class IntegranteViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        //public string Cor { get; set; }
    }
}