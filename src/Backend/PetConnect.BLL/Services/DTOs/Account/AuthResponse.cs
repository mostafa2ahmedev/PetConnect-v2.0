using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Account
{
    public class AuthResponse
    {
        public string? Id { get; set; }
        public string? Token { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Username { get; set; } = string.Empty;
        public List<string>? Roles { get; set; } = new List<string>();
        public DateTime Expiration { get; set; }
    }
}
