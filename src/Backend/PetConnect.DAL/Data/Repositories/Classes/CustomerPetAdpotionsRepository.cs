using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class CustomerPetAdoptionsRepository : GenericRepository<CustomerPetAdoptions>, ICustomerPetAdoptionsRepository
    {
        private readonly AppDbContext context;

        public CustomerPetAdoptionsRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public CustomerPetAdoptions? GetCustomerAdoptionRecord(string RecUserId, string ReqCustomerId, int PetId, string AdoptionDate)
        {
            return context.CustomerPetAdoptions.Where(CPA => CPA.PetId == PetId && CPA.ReceiverCustomerId == RecUserId && CPA.RequesterCustomerId == ReqCustomerId && CPA.Status == Enums.AdoptionStatus.Pending).SingleOrDefault();
        }


        public void RemoveSingleReq(string recUserId, string reqCustomerId, int petId)
        {
            var requests = context.CustomerPetAdoptions
                .Where(cpa => cpa.PetId == petId &&
                              cpa.ReceiverCustomerId == recUserId &&
                              cpa.RequesterCustomerId == reqCustomerId)
            .ToList();

            if (requests.Any())
            {
                context.CustomerPetAdoptions.RemoveRange(requests);
                context.SaveChanges();
            }
        }



        public List<string> RemoveOtherRequestsForPet(int petId, string approvedReqCustomerId)
        {
            // 1️⃣ Get all other requests for this pet (excluding the approved requester)
            var otherRequests = context.CustomerPetAdoptions
                .Where(cpa => cpa.PetId == petId &&
                              cpa.RequesterCustomerId != approvedReqCustomerId && cpa.Status != Enums.AdoptionStatus.Approved)
                .ToList();

            // 2️⃣ Extract all Requester IDs before deletion
            var requesterIds = otherRequests
                .Select(r => r.RequesterCustomerId)
                .Distinct()
                .ToList();

            // 3️⃣ Delete those requests
            if (otherRequests.Any())
            {
                context.CustomerPetAdoptions.RemoveRange(otherRequests);
                context.SaveChanges();
            }

            // 4️⃣ Return the IDs so you can send notifications
            return requesterIds;
        }

    }
}
