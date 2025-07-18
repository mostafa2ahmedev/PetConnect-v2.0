using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Enums
{
    public enum PetSpecialty 
    {
        [Display(Name = "Dog Specialist")]
        Dog,
        [Display(Name = "Cat Specialist")]
        Cat,
        [Display(Name = "Bird Specialist")]
        Bird
    }
}
