using TaskQuest.Models;
using System.Collections.Generic;
using System;

namespace TaskQuest.ViewModels
{
    public class InicioViewModel
    {
        public List<Grupo> Grupos = new List<Grupo>();

        public List<Task> Tasks = new List<Task>();

        public List<Feedback> Feedbacks = new List<Feedback>();
    }

    public class QuestsViewModel
    {
        public List<Quest> Quests = new List<Quest>();

        public List<Tuple<int, Task>> Tasks = new List<Tuple<int, Task>>();
    }
    
}
