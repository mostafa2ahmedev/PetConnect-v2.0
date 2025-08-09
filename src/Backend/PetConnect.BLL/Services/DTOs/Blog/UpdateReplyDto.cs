using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Blog
{
    public class UpdateReplyDto
    {
        [Required]
        public Guid ReplyId { get; set; }
        public string? Reply { get; set; } = null!;
        public IFormFile? Media { get; set; } = null!;
    }
}
