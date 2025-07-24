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
    public class CustomerAddedPetsRepository : GenericRepository<CustomerAddedPets>, ICustomerAddedPetsRepository
    {
        private readonly AppDbContext context;

        public CustomerAddedPetsRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public CustomerAddedPets? GetCustomerAddedPetRecord(int petId, string CustomerId)
        {
            return context.CustomerAddedPets.Where(CAP => CAP.PetId == petId && CAP.CustomerId == CustomerId).SingleOrDefault();
        }
        public int? DeleteCustomerAddedPetRecord(int petId, string CustomerId)
        {
            var record = GetCustomerAddedPetRecord(petId, CustomerId);
            if (record == null)
                return null;
             context.CustomerAddedPets.Remove(record);
             return context.SaveChanges();
        }
    }
}
