using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Product
{
    public  class AddedProductDTO
    {
        [Required(ErrorMessage ="Name is Required")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage ="Description is Required")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage ="Image URL is Required")]
        public IFormFile ImgUrl { get; set; } = null!;
        [Required(ErrorMessage ="Price is Required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Quantity Is Required")]
        public int Quantity { get; set; }

        public int ProductTypeId { get; set; }
    }
}
