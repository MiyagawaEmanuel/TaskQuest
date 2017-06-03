﻿using System;

namespace TaskQuest.ViewModels
{
    public class RegisterViewModel
    {

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Sobrenome { get; set; }

        public DateTime DataNascimento { get; set; }
        
        public string Senha { get; set; }

        public string ConfirmarSenha { get; set; }

        public char Sexo { get; set; }

    }
}