using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.ProductType
{
    public class AddedProductTypeDTO
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        
    }
}
