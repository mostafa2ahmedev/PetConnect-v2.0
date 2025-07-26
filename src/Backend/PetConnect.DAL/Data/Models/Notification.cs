

using PetConnect.DAL.Data.Enums;

namespace PetConnect.DAL.Data.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = null!;
        public NotificationType NotificationType { get; set; }
        public AdoptionNotification AdoptionNotification { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        public Notification()
        {
            Id = Guid.NewGuid();
        }
    }
}
