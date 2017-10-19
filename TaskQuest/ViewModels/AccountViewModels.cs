using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TaskQuest.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
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
        [EmailAddress]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 10)]
        public string LoginEmail { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string LoginSenha { get; set; }
        
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Sobrenome { get; set; }

        [Date]
        [Required]
        public string DataNascimento { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 10)]
        public string Email { get; set; }

        [Required]
        [EmailAddress]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Email")]
        [StringLength(50, MinimumLength = 10)]
        public string ConfirmarEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8)]
        public string Senha { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Senha")]
        public string ConfirmarSenha { get; set; }

        [Required]
        [StringLength(1)]
        [RegularExpression("^M|S$")]
        public string Sexo { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 4)]
        public string Cor { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 10)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password")] 
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ChangePasswordViewModel
    {

        [Required]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.CompareAttribute("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
