using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext context;
        public ProductTypeRepository(AppDbContext _context) :base(_context)
        {
            context = _context;
        }
        public ProductType GetByIDWithProducts(int id)
        {
            return context.ProductType
                          .Include(pt => pt.Products)
                          .FirstOrDefault(pt => pt.Id == id);
        }
    }
}
