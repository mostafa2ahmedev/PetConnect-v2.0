using PetConnect.DAL.Data;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.NewFolder
{
    public class UniqueName : ValidationAttribute
    {
        private readonly IUnitOfWork _unitOfWork;

        public UniqueName(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override bool IsValid(object? value) 
        {
            string? CategoryName = (string?) value;
            bool IsExist = false;
            if (CategoryName != null)
            {
                 IsExist = _unitOfWork.PetCategoryRepository.CheckIfTheCategoryExist(CategoryName);
            }

                if (!IsExist)
                    return true;
                else
                    return false;
                   
                    
        
        }
    }
}
