using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Blog
{
    public class BlogDetails
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = null!;
        public string excerpt { get; set; } = null!;
        public string Media { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int Likes { get; set; }
        public int Comments { get; set; }
        public BlogType BlogType { get; set; }
        public DateTime PostDate { get; set; }
        public string DoctorId { get; set; } = null!;
        public string DoctorName { get; set; } = null!;
        public string DoctorImgUrl { get; set; } = null!;

        public Boolean IsLikedByUser { get; set; }
    }
}
