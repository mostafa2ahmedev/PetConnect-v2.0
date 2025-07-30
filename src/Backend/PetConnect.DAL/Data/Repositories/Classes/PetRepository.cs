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

        public IQueryable<Pet> GetPetBreadCategoryDataWithCustomer()
        {
            return context.Pets.Include(P => P.CustomerAddedPets).ThenInclude(P=>P.Customer).Include(P=>P.Breed).ThenInclude(B=>B.Category);
        }
        public Pet? GetPetDetails(int id)
        {
            return context.Pets
                .Include(p => p.Breed)
                .Include(p => p.Breed.Category)
                .Include(p => p.CustomerAddedPets)
                    .ThenInclude(c => c.Customer)
                .FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Pet> GetPetDataWithCustomer()
        {
            return context.Pets.Include(P => P.CustomerAddedPets).ThenInclude(d => d.Customer);
        }

    }
}
