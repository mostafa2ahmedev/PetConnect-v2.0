using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.PetBreadDto
{
    public class AddedPetBreadDto
    {
        [Required(ErrorMessage ="The Name is Required")]
        public string Name { get; set; } = null!;
        [Display(Name="Category")]
        [Required(ErrorMessage = "The Name is Required")]
        public int CategoryId { get; set; }

    }
}
