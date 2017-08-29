using TaskQuest.Models;
using System.Collections.Generic;

namespace TaskQuest.ViewModels
{
    public class InicioViewModel
    {
        public List<Grupo> Grupos = new List<Grupo>();

        public List<Task> Pendencias = new List<Task>();

        public List<Feedback> Feedbacks = new List<Feedback>();

    }
    
}
