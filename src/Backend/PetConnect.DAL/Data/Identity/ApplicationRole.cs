
using Microsoft.AspNetCore.Identity;

namespace PetConnect.DAL.Data.Identity
{
    public class ApplicationRole:IdentityRole
    {
        public ApplicationRole(string role) : base(role)
        {
            
        }
        public ApplicationRole()
        {
            
        }
    }
}
