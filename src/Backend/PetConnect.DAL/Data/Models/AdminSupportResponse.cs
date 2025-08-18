using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class AdminSupportResponse
    {

        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string? PictureUrl { get; set; } = null!; 
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastActivity { get; set; }
        public int SupportRequestId { get; set; } 
        public SupportRequest Request { get; set; } = null!;
    }
}
