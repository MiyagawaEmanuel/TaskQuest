using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.ViewModels
{    
    public class ContatoViewModel
    {
        public ContatoViewModel() { }

        public ContatoViewModel(User user)
        {
            Nome = user.Nome + " " + user.Sobrenome;

            Cor = user.Cor;
        }

        public ContatoViewModel(Grupo grupo)
        {
            Nome = grupo.Nome;

            Cor = grupo.Cor;
        }

        public string Nome { get; set; }

        public string Cor { get; set; }

        public List<MensagemViewModel> Mensagens = new List<MensagemViewModel>();
    }

    public class MensagemViewModel
    {
        public MensagemViewModel() { }

        public MensagemViewModel(Mensagem mensagem, bool IsUserRemetente, bool IsDestinatarioGrupo)
        {

            Conteudo = mensagem.Conteudo;

            DataMensagem = mensagem.Data;

            IsRemetente = IsUserRemetente;
            
            if(IsDestinatarioGrupo)
            {
                ContatoId = Util.Encrypt(mensagem.UsuarioDestinatarioId.ToString());
                ContatoNome = mensagem.UsuarioDestinatario.Nome + " " + mensagem.UsuarioDestinatario.Sobrenome;
                IsContatoGrupo = false;
            }
            else
            {
                ContatoId = Util.Encrypt(mensagem.GrupoDestinatarioId.ToString());
                ContatoNome = mensagem.GrupoDestinatario.Nome;
                IsContatoGrupo = true;
            }

        }

        public bool IsContatoGrupo { get; set; }

        public string ContatoId { get; set; }

        public string ContatoNome { get; set; }

        public bool IsRemetente { get; set; }

        public DateTime DataMensagem { get; set; }

        [Required]
        [StringLength(1000)]
        public string Conteudo { get; set; }
    }
}