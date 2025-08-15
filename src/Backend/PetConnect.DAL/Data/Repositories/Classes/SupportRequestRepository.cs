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


        public IEnumerable<SupportRequest> GetSupportRequestsWithUserData()
        {
            return context.SupportRequests.Include(SR => SR.User).Where(SR => SR.Status == SupportRequestStatus.Open || SR.Status == SupportRequestStatus.WaitingReply);
        }
    }
}