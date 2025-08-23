using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class UserBlogLike
    {
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; } = null!;

        public DateTime Date { get; set; }

        public CommentORReplyORLikeType UserType { get; set; }
    }
}
