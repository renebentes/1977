using System.ComponentModel.DataAnnotations;

namespace WithoutIdentity.Models.ManagerViewModels
{
    public class IndexViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Display(Name = "Número de Telefone")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        public string Username { get; set; }
    }
}