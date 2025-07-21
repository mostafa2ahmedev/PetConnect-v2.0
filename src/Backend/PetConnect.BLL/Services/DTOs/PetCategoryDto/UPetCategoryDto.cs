using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.PetCategoryDto
{
    public class UPetCategoryDto :IValidatableObject
    {
        [Required(ErrorMessage = "ID is Required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name is Required")]
        public string Name { get; set; } = null!;


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var unitOfWork = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork))!;

            if (unitOfWork.PetCategoryRepository.CheckIfTheCategoryExist(Name))
            {
                yield return new ValidationResult("Category name must be unique", new[] { nameof(Name) });
            }
        }
    }
}
