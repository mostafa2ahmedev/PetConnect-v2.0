using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Doctor
{
    public class ProfileImageDto
    {
        [Required(ErrorMessage = "Profile image is required.")]
        public IFormFile ProfileImage { get; set; }
    }
}
