using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class SupportRequest
    {

        public int Id { get; set; }
        public SupportRequestType Type { get; set; }

        public SupportRequestStatus Status { get; set; }    

        public string Message { get; set; } = null!;

        public string UserId { get; set; } = null!;


        public ApplicationUser User { get; set; } =null!;

        public ICollection<SupportResponse> SupportResponses = new HashSet<SupportResponse>();


  

    }
}
