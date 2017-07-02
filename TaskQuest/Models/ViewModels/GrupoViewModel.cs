using System;
using System.Collections.Generic;
using TaskQuest.Models;

namespace TaskQuest.ViewModels
{
    public class GrupoViewModel
    {

        public Grupo Grupo = new Grupo();

        public List<Tuple<bool, User>> Integrantes = new List<Tuple<bool, User>>();

    }
}