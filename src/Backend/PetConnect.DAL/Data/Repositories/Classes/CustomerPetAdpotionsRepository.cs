using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public CustomerPetAdoptions? GetCustomerAdoptionRecord(string UserId, string RecCustomerId, int PetId)
        {
         return   context.CustomerPetAdoptions.Where(CPA=>CPA.PetId==PetId && CPA.RequesterCustomerId==UserId &&CPA.ReceiverCustomerId==RecCustomerId).FirstOrDefault();
        }
    }
}
