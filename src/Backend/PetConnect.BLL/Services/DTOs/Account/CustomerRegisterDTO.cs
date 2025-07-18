using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.Account
{
    public class CustomerRegisterDTO
    {
        //[Required]
        //public string Role { get; set; }
        //[Required(ErrorMessage = "You Must Enter Your Name")]
        //[MaxLength(50)]
        //[MinLength(3)]
        //public string Name { get; set; }
        //[Required(ErrorMessage = "You Must Enter an Email")]
        ////[Remote("CheckEmail", "User")]
        //[RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        //ErrorMessage = "Please enter a valid email address.")]
        //public string Email { get; set; }

        //[Range(18, 100)]
        //public int Age { get; set; }
        //[Required(ErrorMessage = "Password is required.")]
        //[StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 8)]
        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        //    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        //public string Password { get; set; }

        //[Compare("Password", ErrorMessage = "Passwords do not match.")]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }


        // --- Basic Info ---
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;


        // --- Address Info ---
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; } = null!;

        [Required]
        [Display(Name = "City")]
        public string City { get; set; } = null!;

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; } = null!;
    }
}
