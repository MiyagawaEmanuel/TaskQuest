using TaskQuest.Models;

namespace TaskQuest.ViewModels
{
    public class EditarUsuarioViewModel
    {

        public EditarUsuarioViewModel() { }

        public EditarUsuarioViewModel(User user)
        {
            Nome = user.Nome;
            Sobrenome = user.Sobrenome;
            DataNascimento = user.DataNascimento.ToString("yyyy-MM-dd");
            Email = user.Email;
            Senha = user.PasswordHash;
            ConfirmarSenha = user.PasswordHash;
            Cor = user.Cor;
        }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string DataNascimento { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string ConfirmarSenha { get; set; }

        public string Cor { get; set; }

    }
}