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
    public class ShelterLocationsRepository : GenericRepository<ShelterLocations>, IShelterLocationsRepository
    {
        private readonly AppDbContext context;

        public ShelterLocationsRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
