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
    public class ProductRepository: GenericRepository<Product> , IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext _context):base(_context)
        {
            context = _context;
        }

        public Product? GetProductWithSellerData(int productId) {
            return context.Products.Include(P => P.Seller).SingleOrDefault(P=>P.Id== productId);


        }
        public IEnumerable<Product>? GetAllProductsWithSeller()
        {
            return context.Products.Include(P => P.Seller).ToList();


        }
    }
}
