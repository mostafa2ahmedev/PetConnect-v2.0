using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.BLL.Services.DTO.Account
{
    public class DoctorRegisterDTO
    {
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

        // --- Doctor-specific Info ---
        [Required]
        [Display(Name = "Price Per Hour")]
        [Range(0, double.MaxValue)]
        public decimal PricePerHour { get; set; }

        [Required]
        [Display(Name = "Pet Specialty")]
        public PetSpecialty PetSpecialty { get; set; }

        [Required]
        [Display(Name = "Certificate URL")]
        public string CertificateUrl { get; set; } = null!;

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
