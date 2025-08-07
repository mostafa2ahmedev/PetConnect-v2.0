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
        public string Content { get; set; } = null!;
        public string Media { get; set; } = null!;
        public BlogType BlogType { get; set; }
    }
}
