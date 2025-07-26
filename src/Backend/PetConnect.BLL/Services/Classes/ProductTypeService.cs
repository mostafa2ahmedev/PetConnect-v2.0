using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTOs.Product;
using PetConnect.BLL.Services.DTOs.ProductType;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class ProductTypeService:IProductTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public ProductTypeService(IUnitOfWork unitOfWork , IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }

        public async Task<int> AddProductType(AddedProductTypeDTO addedProductTypeDTO)
        {
            var BreedId = _unitOfWork.PetBreedRepository.GetByID(addedProductTypeDTO.BreedId);
            if (BreedId is null)
                return 0;
            var productType = new ProductType()
            {
                Name = addedProductTypeDTO.Name,
                PetPreedId = addedProductTypeDTO.BreedId
            };
            _unitOfWork.ProductTypeRepository.Add(productType);
            return _unitOfWork.SaveChanges();
            
        }

        public int DeleteProductType(int id)
        {
            var ProductType = _unitOfWork.ProductTypeRepository.GetByID(id);
            if (ProductType is null)
                return 0;
            _unitOfWork.ProductTypeRepository.Delete(ProductType);
            return _unitOfWork.SaveChanges();
        }

        public IEnumerable<ProductTypeDataDTO> GetAllProductsType()
        {
            List<ProductTypeDataDTO> productTypeDataDTOs = new List<ProductTypeDataDTO>();
            var productTypes = _unitOfWork.ProductTypeRepository.GetAll();
            foreach (var productType in productTypes) 
            {
                var breed = _unitOfWork.PetBreedRepository.GetByID(productType.PetPreedId);
                productTypeDataDTOs.Add(new ProductTypeDataDTO()
                {
                    Name = productType.Name,
                    BreedId = productType.PetPreedId,
                    BreedName = breed?.Name?? "Unknown"
                    

                });
            }
            return productTypeDataDTOs;
        }

        public ProductTypeDetailsDTO GetProductTypeDetails(int id)
        {
            var productType = _unitOfWork.ProductTypeRepository.GetByIDWithProducts(id);
            if (productType is null)
                return null;

            var products = productType.Products.Select(p => new ProductDataDTO
            {
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity
            }).ToList();

            var productTypeDetails = new ProductTypeDetailsDTO
            {
                Id = productType.Id,
                Name = productType.Name,
                Products = products
            };

            return productTypeDetails;
        }

        public async Task<int> UpdateProductType(UpdatedProductTypeDTO updatedProductTypeDTO)
        {
            var producttype = _unitOfWork.ProductTypeRepository.GetByID(updatedProductTypeDTO.Id);
            if (producttype is null)
                return 0;
            if (updatedProductTypeDTO.BreedId.HasValue)
            {
                var breed = _unitOfWork.PetBreedRepository.GetByID(updatedProductTypeDTO.BreedId.Value);
                if (breed is null)
                    return -1;

                producttype.PetPreedId = updatedProductTypeDTO.BreedId.Value;
            }
            producttype.Name = updatedProductTypeDTO.Name??producttype.Name;
            producttype.PetPreedId = updatedProductTypeDTO.BreedId??producttype.PetPreedId;
            _unitOfWork.ProductTypeRepository.Update(producttype);
            return _unitOfWork.SaveChanges();
        }
    }
}
