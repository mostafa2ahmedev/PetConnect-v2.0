using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface IOrderProductRepository:IGenericRepository<OrderProduct>
    {
         IEnumerable<OrderProduct> GetProductsByOrderId(int orderId);
         IEnumerable<OrderProduct> GetOrderProductWithProduct_Order_CustomerData(string SellerId);

        OrderProduct? GetOrderProductForSeller(string SellerId,int ProductId ,int OrderId);

        bool CheckStatusOfOrderProductsInOrder(int orderId);
    }
}
