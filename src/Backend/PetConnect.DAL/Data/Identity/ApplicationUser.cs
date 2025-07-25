
using Microsoft.AspNetCore.Identity;

using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;

namespace PetConnect.DAL.Data.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public bool IsApproved { get; set; } 
        public Gender Gender{ get; set; }
        public Address Address { get; set; } = null!;
        public string? ImgUrl{ get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
