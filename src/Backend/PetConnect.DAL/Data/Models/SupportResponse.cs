using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class SupportResponse
    {

        public int Id { get; set; }
        public string Message { get; set; } = null!;

        public int SupportRequestId { get; set; } 
        public SupportRequest Request { get; set; } = null!;
    }
}
