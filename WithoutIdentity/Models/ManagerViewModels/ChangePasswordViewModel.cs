using System.ComponentModel.DataAnnotations;

namespace WithoutIdentity.Models.ManagerViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [Compare(nameof(NewPassword), ErrorMessage = "As senhas devem ser iguais")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ter no mínimo {2} e no máximo {1} caracteres,", MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string OldPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}