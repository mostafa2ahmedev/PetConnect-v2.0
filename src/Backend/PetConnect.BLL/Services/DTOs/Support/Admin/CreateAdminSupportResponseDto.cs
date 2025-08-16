using Microsoft.AspNetCore.Http;
using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Support.Admin
{
    public class CreateAdminSupportResponseDto
    {
        [Required]
        public string Message { get; set; } = null!;
        [Required]
        public string Subject { get; set; } = null!;
        [Required]
        public int SupportRequestId { get; set; }
        [Required]
        public SupportRequestStatus Status { get; set; }

        public IFormFile? PictureUrl { get; set; } = null!;
    }
}