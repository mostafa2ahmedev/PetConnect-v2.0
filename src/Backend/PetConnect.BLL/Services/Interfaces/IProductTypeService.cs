using PetConnect.BLL.Services.DTOs.Product;
using PetConnect.BLL.Services.DTOs.ProductType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IProductTypeService
    {
        Task<int> AddProductType(AddedProductTypeDTO addedProductTypeDTO);
        Task<int> UpdateProductType(UpdatedProductTypeDTO updatedProductTypeDTO);

        int DeleteProductType(int id);

        IEnumerable<ProductTypeDataDTO> GetAllProductsType();

        ProductTypeDetailsDTO GetProductTypeDetails(int id);
    }
}
