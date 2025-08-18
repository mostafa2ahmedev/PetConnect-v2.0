using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PetConnect.BLL.Services.DTOs.Basket;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Classes;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;
using Stripe;

namespace PetConnect.BLL.Services.Classes
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeliveryMethodRepository _deliveryMethodRepository;

        public PaymentService(IConfiguration configuration,
            IBasketRepository basketRepository, IUnitOfWork unitOfWork, IDeliveryMethodRepository deliveryMethodRepository)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _deliveryMethodRepository = deliveryMethodRepository;
        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId , int deliveryMethodId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var Basket = await _basketRepository.GetAsync(BasketId) ?? throw new Exception();
            Basket.deliveryMethodId = deliveryMethodId; // need to be send from front 

            var ProductRepo = _unitOfWork.ProductRepository;

            foreach (var item in Basket.Items)
            {
                var Product = ProductRepo.GetByID(item.Id) ?? throw new Exception();
                item.Price = Product.Price;

            }
            ArgumentNullException.ThrowIfNull(Basket.deliveryMethodId);
            var DeliveryMethod = _deliveryMethodRepository.GetByID(Basket.deliveryMethodId.Value) ?? throw new Exception(); ;

            Basket.shippingPrice = DeliveryMethod.Cost;

            var subtotal = Basket.Items.Sum(item => item.Quantity * item.Price);
            var BasketAmount = (long)((subtotal + DeliveryMethod.Cost) * 100);


            var PaymentService = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (Basket.paymentIntentId is null)
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = BasketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                paymentIntent = await PaymentService.CreateAsync(Options);

                Basket.paymentIntentId = paymentIntent.Id;
                Basket.clientSecret = paymentIntent.ClientSecret;

            }
            else
            {
                var Options = new PaymentIntentUpdateOptions() { Amount = BasketAmount };
                paymentIntent = await PaymentService.UpdateAsync(Basket.paymentIntentId, Options);
                Basket.clientSecret = paymentIntent.ClientSecret;

            }
            await _basketRepository.UpdateAsync(Basket, TimeSpan.FromDays(double.Parse(_configuration.GetSection("RedisSettings")["TimeToLiveInDays"])));

            List<BasketItemDto> basketItemDtos = new List<BasketItemDto>();

            foreach (var item in Basket.Items)
            {
                basketItemDtos.Add(new BasketItemDto()
                {
                    ProductName = item.ProductName,
                    Brand = item.Brand,
                    Category = item.Category,
                    Id = item.Id,
                    PictureUrl = item.PictureUrl,
                    Price = item.Price,
                    Quantity = item.Quantity
                });
            }

            return new CustomerBasketDto()
            {
                clientSecret = Basket.clientSecret,
                deliveryMethodId = Basket.deliveryMethodId,
                Id = Basket.Id,
                Items = basketItemDtos,
                paymentIntentId = Basket.paymentIntentId,
                shippingPrice = Basket.shippingPrice
            };
        }
    }
}
