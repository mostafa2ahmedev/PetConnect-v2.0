using Microsoft.Extensions.Configuration;
using PetConnect.BLL.Services.DTOs.Basket;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,
            IBasketRepository basketRepository,IUnitOfWork unitOfWork,IDel)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var Basket = await _basketRepository.GetAsync(BasketId) ?? throw new Exception();

            var ProductRepo = _unitOfWork.ProductRepository;

            foreach (var item in Basket.Items)
            {
                var Product =  ProductRepo.GetByID(item.Id) ??  throw new Exception();
                item.Price = Product.Price;

            }
            ArgumentNullException.ThrowIfNull(Basket.deliveryMethodId);
            var DeliveryMethod = 
        }
    }
}
