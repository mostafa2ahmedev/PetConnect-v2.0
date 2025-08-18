using Microsoft.Extensions.Configuration;
using PetConnect.BLL.Services.DTOs.Basket;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IConfiguration _configuration;

        public BasketService(IBasketRepository basketRepository, IConfiguration configuration)
        {
            this.basketRepository = basketRepository;
            _configuration = configuration;
        }
        public async Task<CustomerBasketDto> GetCustomerBasketAsync(string basketId)
        {
            var basket = await basketRepository.GetAsync(basketId);
            if (basket is null) throw new Exception();

            List<BasketItemDto> basketItemDtos = new List<BasketItemDto>();
            foreach (var item in basket.Items)
            {
                var basketItemDto = new BasketItemDto()
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Brand = item.Brand,
                    Category = item.Category,
                    PictureUrl = item.PictureUrl,
                    Price = item.Price,
                    Quantity = item.Quantity,

                };
                basketItemDtos.Add(basketItemDto);
            }
            return new CustomerBasketDto()
            {
                Id = basket.Id,
                Items = basketItemDtos,
                paymentIntentId = basket.paymentIntentId
            };
        }
        public async Task<CustomerBasketDto> UpdateCustomerBasketAsync(CustomerBasketDto customerBasket)
        {



            var basket = new CustomerBasket()
            {
                Id = customerBasket.Id,
                Items = customerBasket.Items.Select(item => new BasketItem()
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Brand = item.Brand,
                    Category = item.Category,
                    PictureUrl = item.PictureUrl,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };

            var timeToLive = TimeSpan.FromDays(double.Parse(_configuration.GetSection("RedisSettings")["TimeToLiveInDays"]!));
           
            var updatedBasket = await basketRepository.UpdateAsync(basket,timeToLive);

            if (updatedBasket is null) throw new Exception();

            return customerBasket;
        }

        public async Task DeleteCustomerBasketAsync(string basketId)
        {
            var deleted = await basketRepository.DeleteAsync(basketId);

            if(!deleted) throw new Exception();
        }



    }
}
