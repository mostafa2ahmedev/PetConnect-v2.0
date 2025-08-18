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
    public class CreateFollowUpSupportRequestDto
    {


        [Required(ErrorMessage = "Message Is Required")]
        public string Message { get; set; } = null!;

        public IFormFile? PictureUrl { get; set; } = null!;

        [Required(ErrorMessage = "Support Request Id Is Required")]
        public int SupportRequestId { get; set; }
    }
}
