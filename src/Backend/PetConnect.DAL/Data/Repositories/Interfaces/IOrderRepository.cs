using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        List<Order> GetOrdersWithDetails();
        Order? GetOrderWithDetails(int id);
        List<Order> GetOrdersByCustomerId(string customerId);




    }
}
