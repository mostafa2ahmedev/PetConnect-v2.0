using Microsoft.AspNetCore.Http;
using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.PetDto
{
    public class UpdatedPetDto
    {
        [Required(ErrorMessage = "ID is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Range(0, 50 , ErrorMessage = "Age should be between 0 and 50")]
        public int Age { get; set; }
        [Required(ErrorMessage = "PetStatus is required.")]
        public PetStatus Status { get; set; }
        [Required(ErrorMessage = "Ownership is required.")]
        public Ownership Ownership { get; set; }
        //[Required(ErrorMessage = "ImgURL is required.")]
        public IFormFile? ImgURL { get; set; } = null!;

        [Required(ErrorMessage = "Breed Id is required.")]
        public int BreedId { get; set; }

        public string Notes { get; set; } = null!;
    }
}
