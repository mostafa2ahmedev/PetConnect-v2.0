using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTOs.Order;
using PetConnect.BLL.Services.DTOs.OrderProduct;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentService attachmentService;
        public OrderService(IUnitOfWork _unitOfWork , IAttachmentService _attachmentService)
        {
            unitOfWork = _unitOfWork;
            attachmentService = _attachmentService;
        }
        public async Task<int> AddOrder(AddedOrderDTO addedOrderDTO)
        {
            var order = new Order()
            {
                OrderDate = addedOrderDTO.OrderDate,
                CustomerId = addedOrderDTO.CustomerId,
                TotalPrice = addedOrderDTO.TotalPrice
            };

            unitOfWork.OrderRepository.Add(order);
            unitOfWork.SaveChanges();

            foreach (var product in addedOrderDTO.Products)
            {
                var orderProduct = new OrderProduct()
                {
                    OrderId = order.Id,
                    ProductId = product.ProductId,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity
                };
                unitOfWork.orderProductRepository.Add(orderProduct);
            }
            return unitOfWork.SaveChanges();
        }

        public int DeleteOrder(int id)
        {
            var order = unitOfWork.OrderRepository.GetByID(id);
            if (order is null)
                return 0;
            else
            {
                unitOfWork.OrderRepository.Delete(order);
                return unitOfWork.SaveChanges();
            }
            
        }

        public IEnumerable<OrderDetailsDTO> GetAllOrders()
        {
            
            IEnumerable<Order> orders = unitOfWork.OrderRepository.GetAllWithOrderProduct();
            var orderDetails = orders.Select(d => new OrderDetailsDTO
            {
                OrderDate = d.OrderDate,
                CustomerName = d.customer.FName,
                Products = d.OrderProducts.Select(p => new OrderProductDTO
                {
                    ProductName = p.product.Name,
                    Price = p.product.Price,
                    Quantity = p.product.Quantity
                }).ToList()
            }).ToList();
            return orderDetails;
        }

        public OrderDetailsDTO GetOrderDetails(int id)
        {
           
            Order order = unitOfWork.OrderRepository.GetByID(id);
            if (order is null)
                return null;
            var orderproduct = unitOfWork.orderProductRepository.GetProductsByOrderId(id);
            var products = orderproduct.Select(d => new OrderProductDTO {

                ProductName = d.product.Name,
                Price = d.UnitPrice,
                Quantity = d.Quantity
            }).ToList();
            OrderDetailsDTO orderDetails = new OrderDetailsDTO()
            {
                OrderDate = order.OrderDate,
                CustomerName = order.customer.FName,
                Products = products
            };
            return orderDetails;
        }

        public async Task<int> UpdateOrder(UpdatedOrderDTO updatedOrderDTO)
        {
            var order = unitOfWork.OrderRepository.GetByID(updatedOrderDTO.ID);
            if (order is null)
                return 0;
            order.Id = updatedOrderDTO.ID;
            order.OrderDate = updatedOrderDTO.OrderDate;
            order.CustomerId = updatedOrderDTO.CustomerId;
            unitOfWork.OrderRepository.Update(order);
            var orderProducts = unitOfWork.orderProductRepository.GetProductsByOrderId(order.Id).ToList();
            foreach (var oldproduct in orderProducts)
            {
                var updatedProduct = updatedOrderDTO.Products
        .FirstOrDefault(p => p.ProductId == oldproduct.ProductId);
                if (updatedProduct is null)
                    return 0;
                else
                {
                    oldproduct.UnitPrice = updatedProduct.Price;
                    oldproduct.Quantity = updatedProduct.Quantity;
                    unitOfWork.orderProductRepository.Update(oldproduct);
                }   
            }
            return unitOfWork.SaveChanges();
        
        }
        
    }
}
