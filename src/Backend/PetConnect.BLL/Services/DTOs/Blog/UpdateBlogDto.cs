using Microsoft.AspNetCore.Http;
using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Blog
{
    public class UpdateBlogDto
    {
        public Guid BlogId { get; set; }
        
        public string Content { get; set; } = null!;
        public IFormFile Media { get; set; } = null!;
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string excerpt { get; set; } = null!;
        [Required]
        public BlogType BlogType { get; set; }
    }
}
