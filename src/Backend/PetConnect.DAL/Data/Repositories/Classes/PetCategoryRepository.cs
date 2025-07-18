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
    public class PetCategoryRepository : GenericRepository<PetCategory>, IPetCategoryRepository
    {
        private readonly AppDbContext context;

        public PetCategoryRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
         
        }

        public bool CheckIfTheCategoryExist(string CategoryName)
        {
         return   context.PetCategory.Any(C => C.Name == CategoryName);
        }

        public int GetCountOfCategories()
        {
         return  context.PetCategory.Count();
        }
    }
}
