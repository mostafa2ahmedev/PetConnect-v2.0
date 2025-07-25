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
    public class OrderRepository : GenericRepository<Order> , IOrderRepository
    {
        private readonly AppDbContext context;
        public OrderRepository(AppDbContext _context) :base(_context)
        {
            context = _context;
        }
        public IEnumerable<Order> GetAllWithOrderProduct()
        {
            return context.Orders
                .Include(o => o.customer)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.product)
                .ToList();
        }
    }
}
