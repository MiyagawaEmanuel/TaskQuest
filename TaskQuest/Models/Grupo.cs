using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskQuest.Models
{
    public class Grupo
    {
        public Grupo()
        {
            Quests = new HashSet<Quest>();
            Mensagens = new HashSet<Mensagem>();
            UsuarioGrupos = new HashSet<UsuarioGrupo>();
            DataCriacao = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 4)]
        public string Cor { get; set; } 

        public DateTime DataCriacao { get; set; }

        public bool Plano { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Descricao { get; set; }

        public virtual ICollection<Quest> Quests { get; set; }

        public virtual ICollection<Mensagem> Mensagens { get; set; }

        public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }
        
    }
}
