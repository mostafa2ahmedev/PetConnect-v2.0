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

        [Range(0,50)]
        public int Age { get; set; }
        public PetStatus Status { get; set; }

        public Ownership Ownership { get; set; }

        public IFormFile form { get; set; } = null!;
        public int BreedId { get; set; }

    }
}
