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
        [Required]
        public Guid BlogId { get; set; }
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        public BlogTopic Topic { get; set; }
        public IFormFile? Media { get; set; } = null!;
        [Required]
        public string Title { get; set; } = null!;
     
        public string? excerpt { get; set; } = null!;
        [Required]
        public BlogType BlogType { get; set; }

        [Required]
        public int PetCategoryId { get; set; }
    }
}
