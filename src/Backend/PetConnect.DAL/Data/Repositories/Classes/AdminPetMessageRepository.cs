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
    class AdminPetMessageRepository : GenericRepository<AdminPetMessage> , IAdminPetMessageRepository
    {
        private readonly AppDbContext context;

        public AdminPetMessageRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
