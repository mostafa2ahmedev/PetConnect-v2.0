
using PetConnect.DAL.Data.Enums;

namespace PetConnect.DAL.Data.Models
{
    public class AdminPetMessage
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public AdminMessageType MessageType { get; set; }
        public string Message { get; set; } = null!;
        public int PetId { get; set; }
        public Pet Pet { get; set; } = null!;
    }
}
