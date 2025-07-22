using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.BLL.Services.DTO.Account
{
    public class DoctorRegisterDTO
    {
        [Required(ErrorMessage = "First Name Is Required")]
        [Display(Name = "First Name")]
        public string FName { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "Last Name Is Required")]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Must Be in Email Format like 'Example@gmail.com'")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Didn't Match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmationPassword { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "Gender Is Required")]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "Image Is Required")]
        [Display(Name = "Doctor Image")]
        public IFormFile Image { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "Certificate Is Required")]
        [Display(Name = "Certificate")]
        public IFormFile Certificate { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "Price Per Hour Is Required")]
        [Display(Name = "Price Per Hour")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal PricePerHour { get; set; }
        /*-----------------------------------------------------------------------------------------------------------------*/
        [Display(Name = "Phone Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Number Can Only Have Digits")]
        [Phone(ErrorMessage = "Invalid Phone Number Format")]
        public string PhoneNumber { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/
        [Required(ErrorMessage = "Specialty Is Required")]
        [Display(Name = "Pet Specialty")]
        public PetSpecialty PetSpecialty { get; set; }
        /*-----------------------------------------------------------------------------------------------------------------*/
        [Required(ErrorMessage = "Country Is Required")]
        [Display(Name = "Country")]
        public string Country { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "City Is Required")]
        [Display(Name = "City")]
        public string City { get; set; } = null!;
        /*-----------------------------------------------------------------------------------------------------------------*/

        [Required(ErrorMessage = "Street Is Required")]
        [Display(Name = "Street")]
        public string Street { get; set; } = null!;
    }
}
