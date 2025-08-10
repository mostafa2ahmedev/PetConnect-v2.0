using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Blog
{
    public class CommentDataDto
    {
        public Guid ID { get; set; }
        public string Comment { get; set; } = null!;
        public string Media { get; set; } = null!;
        public string PosterName { get; set; } = null!;
        public string PosterImage { get; set; } = null!;
        public string PosterId { get; set; } = null!;
        public int NumberOfLikes { get; set; }
        public int NumberOfReplies { get; set; }
        public bool IsDeleted { get; set; }
        public Boolean IsLikedByUser { get; set; }


    }
}
