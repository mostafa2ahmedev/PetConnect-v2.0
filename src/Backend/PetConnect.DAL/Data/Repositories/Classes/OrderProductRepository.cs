using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class OrderProductRepository:GenericRepository<OrderProduct> , IOrderProductRepository
    {
        private readonly AppDbContext context;
        public OrderProductRepository(AppDbContext _context):base(_context)
        {
            context = _context;
        }
        public IEnumerable<OrderProduct> GetProductsByOrderId(int orderId)
        {
            return context.orderProducts
                .Include(op => op.product)
                .Where(op => op.OrderId == orderId)
                .ToList();
        }
    }
}
