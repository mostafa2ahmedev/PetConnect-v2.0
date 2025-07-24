using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class ProductTypeRepository: GenericRepository<ProductType> , IProductTypeRepository
    {
        public ProductTypeRepository(AppDbContext _context) :base(_context)
        {
            
        }
    }
}
