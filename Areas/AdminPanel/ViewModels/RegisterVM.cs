using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Mamba.Areas.AdminPanel.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MinLength(3,ErrorMessage ="Adiniz minum 3 simvoldan ibaret olmalidi")]
        public string Name { get; set; }
        [Required]

        [MinLength(3, ErrorMessage = "Adiniz minum 3 simvoldan ibaret olmalidi")]
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password),Compare(nameof(ConfirmPassword))]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        [MinLength(8)]
        public string ConfirmPassword { get; set; }

    }
}
