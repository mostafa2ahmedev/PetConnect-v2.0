

using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;

namespace PetConnect.DAL.Data.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = null!;
        public NotificationType NotificationType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User{ get; set; }

        public Notification()
        {
            Id = Guid.NewGuid();
        }
    }
}
