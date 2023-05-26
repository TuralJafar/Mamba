using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.AdminPanel.ViewModels
{
    public class LoginVM
    {
        public string SurnameOrEmail { get; set; }
       
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
