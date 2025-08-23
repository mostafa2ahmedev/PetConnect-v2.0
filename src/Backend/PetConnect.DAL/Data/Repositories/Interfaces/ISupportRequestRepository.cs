using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface ISupportRequestRepository  :IGenericRepository<SupportRequest>
    {
         IEnumerable<SupportRequest> GetSupportRequestsWithUserDataForAdmin();
         IEnumerable<SupportRequest> GetSupportRequestsForUser(string userId);
         SupportRequest? GetSupportRequestsDetailsForUser(int supportRequestId);

         SupportRequest? GetUesrByRequestId(int SuppRquestId);
    }
}
