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
    public class SellerRepository : GenericRepository<Seller>, ISellerRepository
    {
        private readonly AppDbContext context;

        public SellerRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public string? GetSellerByProductInfo(int ProductId)
        {
       return   context.Products.SingleOrDefault(P => P.Id == ProductId)?.SellerId;
        }
    }
}
