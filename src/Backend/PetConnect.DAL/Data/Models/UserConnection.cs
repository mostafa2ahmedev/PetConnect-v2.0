using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Identity;

namespace PetConnect.DAL.Data.Models
{
    public class UserConnection
    {
        public string ConnectionId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!; 
    }
}
