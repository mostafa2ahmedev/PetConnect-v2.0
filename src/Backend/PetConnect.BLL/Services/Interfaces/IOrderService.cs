using PetConnect.BLL.Services.DTOs.NewFolder;
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
        #region Legacy Code (5ara)
        int AddOrder(AddedOrderDTO dto);

        List<OrderDetailsDTO> GetAllOrders();
        OrderDetailsDTO? GetOrderDetails(int id);
        int UpdateOrder(UpdatedOrderDTO dto);
        int DeleteOrder(int id);
        List<OrderDetailsDTO> GetOrdersByCustomer(string customerId);
        #endregion

        Task<OrderToReturnDto> CreateOrderAsync(string UserId, string buyerEmail, OrderToCreateDto order);

        Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId);
        Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail);


        IEnumerable<DeliveryMethodDto> GetDeliveryMethodsAsync();

    }
}
