using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Identity;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class AdminSupportResponseRepository : GenericRepository<AdminSupportResponse>, IAdminSupportResponseRepository
    {
        private readonly AppDbContext context;

        public AdminSupportResponseRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }



    }
}