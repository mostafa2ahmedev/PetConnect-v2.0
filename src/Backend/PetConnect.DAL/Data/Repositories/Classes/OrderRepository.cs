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
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Order> GetOrdersWithDetails()
        {
            return _context.Orders
                .Include(o => o.customer)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.product)
                .ToList();
        }

        public Order? GetOrderWithDetails(int id)
        {
            return _context.Orders
                .Include(o => o.customer)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.product)
                .FirstOrDefault(o => o.Id == id);
        }
        public List<Order> GetOrdersByCustomerId(string customerId)
        {
            return _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.product)
                .ToList();
        }

    }
}
