using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class SupportRequestRepository : GenericRepository<SupportRequest>, ISupportRequestRepository
    {
        private readonly AppDbContext context;

        public SupportRequestRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }


  

        public IEnumerable<SupportRequest> GetSupportRequestsWithUserDataForAdmin()
        {
            return context.SupportRequests.Include(SR => SR.User);
        }


        public IEnumerable<SupportRequest> GetSupportRequestsForUser(string userId)
        {
            return context.SupportRequests.Where(S => S.UserId == userId);
        }
        public SupportRequest? GetSupportRequestsDetailsForUser(int supportRequestId)
        {
            return context.SupportRequests.Include(S=>S.User).Include(S=>S.AdminSupportResponses).Include(S=>S.FollowUpSupportRequests).SingleOrDefault(S=>S.Id ==supportRequestId);
        }
        public SupportRequest? GetUesrByRequestId(int SuppRquestId)
        {

            return context.SupportRequests.SingleOrDefault(SR => SR.Id == SuppRquestId);
        }
    }
}