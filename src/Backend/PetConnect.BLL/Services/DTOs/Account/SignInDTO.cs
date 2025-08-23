using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.Account
{
    public class SignInDTO
    {
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        /*----------------------------------------------------------*/
        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        /*----------------------------------------------------------*/
        [Display(Name = "Remember Me !!")]
        public bool RememberMe { get; set; }
    }
}
