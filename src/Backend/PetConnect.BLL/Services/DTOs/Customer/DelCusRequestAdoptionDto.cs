using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Customer
{
    public class DelCusRequestAdoptionDto
    {

        [Required(ErrorMessage = "Customer Id Can't be null")]
        public string RecCustomerId { get; set; } = null!;
        [Required(ErrorMessage = "Pet Id Can't be null")]
        public int PetId { get; set; }
        [Required(ErrorMessage = "Adoption Date Can't be null")]
        public string AdoptionDate { get; set; } = null!;
    }
}
