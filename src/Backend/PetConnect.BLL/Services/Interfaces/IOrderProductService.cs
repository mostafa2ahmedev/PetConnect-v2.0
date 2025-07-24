using PetConnect.BLL.Services.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IOrderProductService
    {
        Task<int> AddOrderProduct(AddedProductDTO addedProductDTO);
        Task<int> UpdateOrderProduct(UpdatedProductDTO updatedProductDTO);

        int DeleteOrderProduct(int id);

        IEnumerable<ProductDetailsDTO> GetAllOrderProduct();

        ProductDetailsDTO GetOrderProductDetails(int id);
    }
}
