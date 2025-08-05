using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.DTOs.OrderProduct;
using PetConnect.BLL.Services.DTOs.Product;
using PetConnect.BLL.Services.DTOs.Seller;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface ISellerService
    {

         SellerDetailsDto? GetProfile(string id);
         IEnumerable<SellerDataDto> GetAllSellers();
         int Delete(string id);
        Task<int> UpdateProfile(string SellerId,UpdateSellerProfileDto customerProfileDTO);

        IEnumerable<SellerProductsDto> GetAllProducts(string sellerId);



    }
}
