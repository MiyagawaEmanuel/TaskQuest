﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaskQuest.Models;

namespace TaskQuest.ViewModels
{
    public class GrupoViewModel
    {

        public GrupoViewModel() { }

        public GrupoViewModel(Grupo grupo)
        {
            this.Id = Util.Hash(grupo.Id.ToString());
            this.Nome = grupo.Nome;
            this.Descricao = grupo.Descricao;
            this.Plano = grupo.Plano;
        }

        public Grupo Update()
        {
            if (string.IsNullOrEmpty(this.Id))
            {

                Grupo grupo = new Grupo()
                {
                    Nome = this.Nome,
                    Descricao = this.Descricao,
                    DataCriacao = System.DateTime.Now,
                    Plano = false,
                    Cor = this.Cor,
                };

                return grupo;
            }
            else
            {
                using (var db = new DbContext())
                {
                    var aux = db.Grupo.ToList().Where(q => Util.Hash(q.Id.ToString()) == this.Id);
                    if (aux.Any())
                    {
                        Grupo grupo = aux.First();

                        grupo.Nome = this.Nome;
                        grupo.Descricao = this.Descricao;
                        grupo.Plano = false;
                        grupo.Cor = this.Cor;

                        return grupo;
                    }
                    else
                        return null;
                }
            }
        }

        public string Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Descricao { get; set; }

        [Required]
        public bool Plano { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 4)]
        public string Cor { get; set; }
        
        public List<User> Integrantes = new List<User>();

        public List<Quest> Quests = new List<Quest>();

    }

    public class EditarIntegranteViewModel
    {

        [Required]
        public string UserId { get; set; }

        [Required]
        public string GrupoId { get; set; }

    }

}

