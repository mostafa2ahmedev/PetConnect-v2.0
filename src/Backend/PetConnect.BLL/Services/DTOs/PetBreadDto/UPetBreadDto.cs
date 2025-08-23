using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.PetBreadDto
{
    public class UPetBreedDto
    {
        [Required(ErrorMessage = "ID is Required")]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}
