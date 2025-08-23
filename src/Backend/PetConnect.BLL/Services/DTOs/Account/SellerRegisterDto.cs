using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Account
{
    public class SellerRegisterDto
    {
        [Required(ErrorMessage = "First Name Is Required")]
        [Display(Name = "First Name")]
        public string FName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name Is Required")]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = null!;

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Must Be in Email Format like 'Example@gmail.com'")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email Is Already Used")]
        public string Email { get; set; } = null!;
        [Display(Name = "Phone Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Number Can Only Has Digits")]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
        
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        
        [Required(ErrorMessage = "Confirm Password Is Required")]

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Didn't Match")]
        public string ConfirmationPassword { get; set; } = null!;
        
        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }
        
        public IFormFile Image { get; set; } = null!;
        
        [Required]
        public string Country { get; set; } = null!;
        
        [Required]
        public string City { get; set; } = null!;
        
        [Required]
        public string Street { get; set; } = null!;
    }
}
