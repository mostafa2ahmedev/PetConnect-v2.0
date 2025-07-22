using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PetConnect.BLL.Services.DTOs.Customer
{
    public class CustomerProfileDTO
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = null!;

        public string ImgUrl { get; set; } = null!; // current image URL (used when no new image is uploaded)

        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; } // for upload

        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "Street is required.")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = null!;
    }
}
