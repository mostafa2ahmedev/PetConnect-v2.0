using PetConnect.BLL.Services.DTOs.OrderProduct;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class OrderProductService : IOrderProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

     

        public IEnumerable<OrderProductForSellerConfirmationDto> GetOrderProductForSellerConfirmationDto(string SellerId)
        {
            return _unitOfWork.orderProductRepository
                .GetOrderProductWithProduct_Order_CustomerData(SellerId)
                .Select(OP => new OrderProductForSellerConfirmationDto()
                {
                    OrderId = OP.OrderId,
                    CustomerId = OP.order.CustomerId,
                    ProductId = OP.ProductId,
                    CustomerName = OP.order.customer.FName + " " + OP.order.customer.LName,
                    OrderDate = OP.order.OrderDate,
                    ProductName = OP.product.Name,
                    Quantity = OP.Quantity,
                    UnitPrice = OP.UnitPrice,
                    OrderProductStatus = OP.OrderProductStatus,
                    ProductImgUrl = OP.product.ImgUrl

                });
        }

        public int? ShippingOrDenyingOrderProductInOrder(string SellerId, ShipOrDenyOrderProductDto shipOrDenyOrderProductDto)
        {


            var OrderProduct=  _unitOfWork.orderProductRepository.GetOrderProductForSeller(SellerId,shipOrDenyOrderProductDto.ProductId,shipOrDenyOrderProductDto.OrderId);
            if (OrderProduct == null)
                return null;

            OrderProduct.OrderProductStatus=shipOrDenyOrderProductDto.OrderProductStatus;

            if (OrderProduct.OrderProductStatus == OrderProductStatus.Shipped)
                DecreaseQuantityOfProduct(OrderProduct.ProductId, OrderProduct.Quantity);

            var result= _unitOfWork.SaveChanges();


            
            var IsAnyProductPending=  CheckIfThereIsAnyOrderProductWithStatusPendingForSpecificProduct(shipOrDenyOrderProductDto.OrderId);
          
            if (!IsAnyProductPending) {
                    Order? order = _unitOfWork.OrderRepository.GetByID(shipOrDenyOrderProductDto.OrderId);

                if (order is { })                                                    
                {                                                                    
                    order.OrderStatus = OrderStatus.Shipped;                         
                    _unitOfWork.OrderRepository.Update(order);                       
                    _unitOfWork.SaveChanges();                                       
                }                                                                    
                
            }
            return result;
                
        }



        public bool CheckIfThereIsAnyOrderProductWithStatusPendingForSpecificProduct(int OrderId)
        {
        return  _unitOfWork.orderProductRepository.CheckStatusOfOrderProductsInOrder(OrderId);
        }


        public bool DecreaseQuantityOfProduct(int ProductId, int QuantityNeedsToDecrease)
        {
            var Product = _unitOfWork.ProductRepository.GetByID(ProductId);
            if (Product == null) return false;

            if (Product.Quantity >= QuantityNeedsToDecrease) {
                Product.Quantity -= QuantityNeedsToDecrease;
                return true;
            }
            return false;

     
               
        }
    }
}
