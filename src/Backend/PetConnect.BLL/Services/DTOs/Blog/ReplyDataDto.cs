using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Blog
{
    public class ReplyDataDto
    {
        public Guid ID { get; set; }
        public string Reply { get; set; } = null!;
        public string Media { get; set; } = null!;
        public string PosterName { get; set; } = null!;
        public string PosterImage { get; set; } = null!;
        public int NumberOfLikes { get; set; }

        public bool IsDeleted { get; set; }
        public Boolean IsLikedByUser { get; set; }

    }
}
