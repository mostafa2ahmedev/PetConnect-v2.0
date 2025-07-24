using Microsoft.AspNetCore.Http;
using PetConnect.DAL.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PetConnect.BLL.Services.DTOs.Customer
{
    public class UpdateCustomerProfileDTO
    {

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = null!;

        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; } 

        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "City is required.")]
        public string Country { get; set; } = null!; 
    }
}
