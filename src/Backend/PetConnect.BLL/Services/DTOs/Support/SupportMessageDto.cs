using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Support
{
    public class SupportMessageDto
    {

        public string Sender { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? PictureUrl { get; set; } = null!;
        public string? CreatedAt { get; set; } = null!;  
    }
}
