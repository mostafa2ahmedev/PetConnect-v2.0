using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Identity;
using PetConnect.DAL.Data.Repositories.Interfaces;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly AppDbContext context;

        public ApplicationUserRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
