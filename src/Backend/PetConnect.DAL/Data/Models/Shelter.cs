
namespace PetConnect.DAL.Data.Models
{
    public class Shelter
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OwnerId { get; set; } = null!;

        public ICollection<ShelterAddedPets> ShelterAddedPets { get; set; } = new HashSet<ShelterAddedPets>();
        public ICollection<ShelterPetAdoptions> ShelterPetAdoptions { get; set; } = new HashSet<ShelterPetAdoptions>();

        public ShelterOwner ShelterOwner { get; set; } = null!;

        public ICollection<ShelterImages> ShelterImages = new HashSet<ShelterImages>();

        public ICollection<ShelterPhones> ShelterPhones = new HashSet<ShelterPhones>();

        public ICollection<ShelterLocations> ShelterLocations = new HashSet<ShelterLocations>();
    }
}
