using Microsoft.AspNetCore.Http;
using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Support.User
{
    public class CreateSupportRequestDto
    {
        [Required(ErrorMessage ="Type Is Required")]
        public SupportRequestType Type { get; set; }
        [Required(ErrorMessage = "Message Is Required")]
        public string Message { get; set; } = null!;

        public IFormFile? PictureUrl { get; set; } = null!;
    }
}
