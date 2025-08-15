using PetConnect.BLL.Services.DTOs.Address;
using PetConnect.BLL.Services.DTOs.NewFolder;
using PetConnect.BLL.Services.DTOs.Order;
using PetConnect.BLL.Services.DTOs.OrderProduct;
using PetConnect.BLL.Services.DTOs.Product;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;
using System.Collections.Generic;
using System.Linq;

namespace PetConnect.BLL.Services.Classes
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IUnitOfWork unitOfWork , IBasketService basketService,IProductRepository productRepository , IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public async Task<OrderToReturnDto> CreateOrderAsync(string UserId,string buyerEmail, OrderToCreateDto order)
        {
            var basket = await _basketService.GetCustomerBasketAsync(order.BasketId);
            var orderItems = new List<OrderProduct>();
            if (basket.Items.Count > 0) {
              
                foreach (var item in basket.Items)
                {
                    var product = _productRepository.GetByID(item.Id);

                    if(product is not null) {
                        var productItemOrdered = new ProductItemOrdered()
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            PictureUrl = product.ImgUrl ?? ""

                        };

                        var orderItem = new OrderProduct() {
                        product = product,
                        UnitPrice = product.Price,
                        Quantity = item.Quantity,
                        SellerId= product.SellerId,
                        OrderProductStatus = OrderProductStatus.Pending,
                        ProductId = product.Id,
                        
                        
                        
                        };
                        orderItems.Add(orderItem);
                    }

                }
                var subTotal = orderItems.Sum(item => item.UnitPrice * item.Quantity);
                var User = _userRepository.GetByID(UserId);
                var orderToCreate = new Order()
                {
                    OrderProducts = orderItems,
                    DeliveryMethodId = order.DeliveryMethodId,
                    OrderStatus = OrderStatus.Pending,
                    CustomerId = UserId,
                    SubTotal = subTotal,
                    ShippingAddress = new Address()
                    {
                        City = User.Address.City,
                        Country = User.Address.Country,
                        Street = User.Address.Street,
                    }

                };
                _unitOfWork.OrderRepository.Add(orderToCreate);

                var created = await _unitOfWork.SaveChangesAsync() > 0;

                if (!created) throw new Exception();

                var orderitemlistDtos = new List<OrderItemDto>();

                foreach (var item in orderToCreate.OrderProducts)
                {
                    orderitemlistDtos.Add(new OrderItemDto()
                    {
                        Id = item.ProductId,
                        Price = item.UnitPrice,
                        Quantity = item.Quantity,


                    });

                }

                return new OrderToReturnDto()
                {

                    BuyerEmail = buyerEmail,
                    OrderItems = orderitemlistDtos,
                    SubTotal = subTotal,
                    Status = (orderToCreate.OrderStatus).ToString(),
                    DeliveryMethodId = orderToCreate.DeliveryMethodId,
                    ShippingAddress = new AddressDto()
                    {
                        City = User.Address.City,
                        Country = User.Address.Country,
                        Street = User.Address.Street
                    }
                ,
                    OrderDate = orderToCreate.OrderDate,
                    Id = orderToCreate.Id,
                    DeliveryMethod = orderToCreate?.DeliveryMethod?.ShortName,
                    Total  = orderToCreate!.Total
                };
            }
            throw new Exception();

        }

        public Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }
        #region Legacy Code (5ara)

        public int AddOrder(AddedOrderDTO dto)
        {



            var order = new Order
            {
                OrderDate = dto.OrderDate,
                CustomerId = dto.CustomerId,
                OrderStatus = OrderStatus.Pending,
                SubTotal = dto.Products.Sum(p => p.Quantity * p.UnitPrice)
            };

            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.SaveChanges();


            foreach (var p in dto.Products)
            {
                var sellerId = _unitOfWork.SellerRepository.GetSellerByProductInfo(p.ProductId);


                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice,
                    OrderProductStatus = OrderProductStatus.Pending,
                    SellerId = sellerId
                };

                _unitOfWork.orderProductRepository.Add(orderProduct);
            }

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
                    ProductImageUrl = op.product?.ImgUrl,
                    Status = op.OrderProductStatus.ToString()

                }).ToList()
            }).ToList();
        }

    
        #endregion

    }
}
