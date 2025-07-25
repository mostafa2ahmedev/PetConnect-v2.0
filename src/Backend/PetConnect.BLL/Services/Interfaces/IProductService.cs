using PetConnect.BLL.Services.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<int> AddProduct(AddedProductDTO addedProductDTO);
        Task<int> UpdateProduct(UpdatedProductDTO updatedProductDTO);

        int DeleteProduct(int id);

        IEnumerable<ProductDetailsDTO> GetAllProducts();

        ProductDetailsDTO GetProductDetails(int id);
    }
}
