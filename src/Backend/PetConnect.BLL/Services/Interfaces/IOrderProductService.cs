using PetConnect.BLL.Services.DTOs.OrderProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IOrderProductService
    {
        IEnumerable<OrderProductForSellerConfirmationDto> GetOrderProductForSellerConfirmationDto(string SellerId);

        int? ShippingOrDenyingOrderProductInOrder(string SellerId, ShipOrDenyOrderProductDto shipOrDenyOrderProductDto);

        bool CheckIfThereIsAnyOrderProductWithStatusPendingForSpecificProduct(int OrderId);

        bool DecreaseQuantityOfProduct(int ProductId,int QuantityNeedsToDecrease);
    }
}
