
using PetConnect.DAL.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetConnect.DAL.Data.Models
{
    public class Pet
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; } 
        public PetStatus Status { get; set; }
        public bool IsApproved { get; set; }
        public Ownership Ownership{ get; set; }
        public string ImgUrl { get; set; } = null!;
        public string Notes { get; set; } = null!;
        public bool IsDeleted { get; set; }

        //Navs


        public int BreedId { get; set; }
        public PetBreed Breed{ get; set; } = null!; // Set No Action 

        public CustomerAddedPets CustomerAddedPets { get; set; } = null!;
        public ShelterAddedPets ShelterAddedPets { get; set; } = null!;

        public ICollection<ShelterPetAdoptions> ShelterPetAdoptions { get; set; } = new HashSet<ShelterPetAdoptions>();
        public ICollection<CustomerPetAdoptions> CustomerPetAdoptions { get; set; } = new HashSet<CustomerPetAdoptions>();
        public ICollection<AdminPetMessage> AdminPetMessages { get; set; } = new HashSet<AdminPetMessage>();



    }
}
