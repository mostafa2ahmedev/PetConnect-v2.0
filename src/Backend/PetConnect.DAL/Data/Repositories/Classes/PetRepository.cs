using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        private readonly AppDbContext context;

        public PetRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
           
        }

        public IQueryable<Pet> GetPendingPetsWithBreedAndCategory()
        {
            return context.Pets
                .Include(p => p.Breed)
                    .ThenInclude(b => b.Category)
                .Where(p => !p.IsApproved);
        }

        public Pet? GetPetDataWithCustomer(int id)
        {
            return context.Pets.Include(P => P.CustomerAddedPets).FirstOrDefault(p => p.Id == id);
        }
    }
}
