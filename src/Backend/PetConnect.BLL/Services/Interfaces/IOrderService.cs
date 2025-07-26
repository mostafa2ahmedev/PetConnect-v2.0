using PetConnect.BLL.Services.DTOs.Order;
using PetConnect.BLL.Services.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> AddOrder(AddedOrderDTO addedOrderDTO);
        Task<int> UpdateOrder(UpdatedOrderDTO updatedOrderDTO);

        int DeleteOrder(int id);

        IEnumerable<OrderDetailsDTO> GetAllOrders();

        OrderDetailsDTO GetOrderDetails(int id);
    }
}
