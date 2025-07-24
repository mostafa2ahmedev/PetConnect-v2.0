using PetConnect.BLL.Services.DTOs.Product;
using PetConnect.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class OrderProductService : IOrderProductService
    {
        public Task<int> AddOrderProduct(AddedProductDTO addedProductDTO)
        {
            throw new NotImplementedException();
        }

        public int DeleteOrderProduct(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductDetailsDTO> GetAllOrderProduct()
        {
            throw new NotImplementedException();
        }

        public ProductDetailsDTO GetOrderProductDetails(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateOrderProduct(UpdatedProductDTO updatedProductDTO)
        {
            throw new NotImplementedException();
        }
    }
}
