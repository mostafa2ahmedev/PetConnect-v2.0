using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class SupportRequest
    {

        public int Id { get; set; }
        public SupportRequestType Type { get; set; }
        public SupportRequestStatus Status { get; set; }
        public SupportRequestPriority Priority { get; set; }
        public string? PictureUrl { get; set; } = null!;
        public string Message { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastActivity { get; set; }

        public ApplicationUser User { get; set; } =null!;

        public ICollection<AdminSupportResponse> AdminSupportResponses = new HashSet<AdminSupportResponse>();

        public ICollection<FollowUpSupportRequest> FollowUpSupportRequests = new HashSet<FollowUpSupportRequest>();



    }
}
