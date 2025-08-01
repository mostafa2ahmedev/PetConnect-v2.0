using PetConnect.BLL.Services.DTOs.Order;
using PetConnect.BLL.Services.DTOs.OrderProduct;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System.Collections.Generic;
using System.Linq;

namespace PetConnect.BLL.Services.Classes
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int AddOrder(AddedOrderDTO dto)
        {
            var order = new Order
            {
                OrderDate = dto.OrderDate,
                CustomerId = dto.CustomerId,
                OrderProducts = dto.Products.Select(p => new OrderProduct
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice
                }).ToList()
            };

            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.SaveChanges();

            return order.Id;
        }

        public List<OrderDetailsDTO> GetAllOrders()
        {
            var orders = _unitOfWork.OrderRepository.GetOrdersWithDetails();

            return orders.Select(o => new OrderDetailsDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Products = o.OrderProducts.Select(op => new OrderProductDTO
                {

                    ProductName = op.product?.Name ?? "Unknown",
                    Quantity = op.Quantity,
                    Price = op.UnitPrice,
                    ProductImageUrl = op.product?.ImgUrl

                }).ToList()
            }).ToList();
        }

        public OrderDetailsDTO? GetOrderDetails(int id)
        {
            var order = _unitOfWork.OrderRepository.GetOrderWithDetails(id);
            if (order == null) return null;

            return new OrderDetailsDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Products = order.OrderProducts.Select(op => new OrderProductDTO
                {
                    ProductName = op.product?.Name ?? "Unknown",
                    Quantity = op.Quantity,
                    Price = op.UnitPrice,
                    ProductImageUrl = op.product?.ImgUrl
                }).ToList()
            };
        }

        public int UpdateOrder(UpdatedOrderDTO dto)
        {
            var existingOrder = _unitOfWork.OrderRepository.GetOrderWithDetails(dto.ID);
            if (existingOrder == null) return 0;

            existingOrder.OrderDate = dto.OrderDate;
            existingOrder.CustomerId = dto.CustomerId;

            // Remove old products
            existingOrder.OrderProducts.Clear();

            // Add new ones
            foreach (var item in dto.Products)
            {
                existingOrder.OrderProducts.Add(new OrderProduct
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }

            _unitOfWork.OrderRepository.Update(existingOrder);
            _unitOfWork.SaveChanges();

            return existingOrder.Id;
        }

        public int DeleteOrder(int id)
        {
            var order = _unitOfWork.OrderRepository.GetByID(id);
            if (order == null) return 0;

            _unitOfWork.OrderRepository.Delete(order);
            _unitOfWork.SaveChanges();

            return id;
        }

        public List<OrderDetailsDTO> GetOrdersByCustomer(string customerId)
        {
            var orders = _unitOfWork.OrderRepository.GetOrdersByCustomerId(customerId);

            return orders.Select(o => new OrderDetailsDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Products = o.OrderProducts.Select(op => new OrderProductDTO
                {
                    ProductName = op.product?.Name ?? "Unknown",
                    Quantity = op.Quantity,
                    Price = op.UnitPrice,
                    ProductImageUrl = op.product?.ImgUrl

                }).ToList()
            }).ToList();
        }

    }
}
