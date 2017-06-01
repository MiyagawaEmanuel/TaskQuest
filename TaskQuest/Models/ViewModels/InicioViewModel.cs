using System;
using System.Collections.Generic;

namespace TaskQuest.ViewModels
{
    public class InicioViewModel
    {
        List<Grupo> Grupos = new List<Grupo>();
        List<Pendencia> Pendencias = new List<Pendencia>();
        List<Feedback> Feedbacks = new List<Feedback>();
    }

    public class Grupo
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Cor { get; set; }
    }
    
    public class Pendencia
    {
        public string Namo { get; set; }

        public string Cor { get; set; }

        public DateTime Data { get; set; }

        public string Descricao { get; set; }

        public int Status { get; set; }
    }

    public class Feedback
    {
        public string Nome { get; set; }

        public string Cor { get; set; }

        public DateTime Data { get; set; }

        public int Nota { get; set; }

        public string Resposta { get; set; }
    }
}
