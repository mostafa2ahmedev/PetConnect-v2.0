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
    public class AddedPetDto
    {

        [Required(ErrorMessage = "The Name is required")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(0,50, ErrorMessage = "Age should be between 0 and 50")]
        public int Age { get; set; }

        [Required(ErrorMessage ="Status can't be null")]
        public PetStatus Status { get; set; }
        [Required(ErrorMessage = "Ownership can't be null")]
        public Ownership Ownership { get; set; }
        [Required(ErrorMessage = "ImgURL can't be null")]
        public IFormFile ImgURL { get; set; } = null!;
        [Required(ErrorMessage = "Breed Id can't be null")]
        public int BreedId { get; set; }
        public string? Notes { get; set; }
    }
}
