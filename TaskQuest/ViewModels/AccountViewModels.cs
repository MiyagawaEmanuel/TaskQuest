using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TaskQuest.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberBrowser { get; set; }

        [HiddenInput]
        public int UserId { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }

        [HiddenInput]
        public int UserId { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {

        public string LoginEmail { get; set; }

        public string LoginSenha { get; set; }
        
    }

    public class RegisterViewModel
    {
        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string DataNascimento { get; set; }
        
        [EmailAddress]
        [StringLength(40, MinimumLenght = 10)]
        public string Email { get; set; }

        [EmailAddress]
        [StringLength(40, MinimumLenght = 10)]
        public string ConfirmarEmail { get; set; }

        public string Senha { get; set; }

        public string ConfirmarSenha { get; set; }

        public string Sexo { get; set; }

        public string Cor { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]  
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
