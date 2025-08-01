using Microsoft.AspNetCore.Components.Forms;
using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Product;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentService attachmentService;
        public ProductService(IUnitOfWork _unitOfWork , IAttachmentService _attachmentService)
        {
            unitOfWork = _unitOfWork;
            attachmentService = _attachmentService;
        }
        public async Task<int> AddProduct(AddedProductDTO addedProductDTO)
        {
            var image = await attachmentService.UploadAsync(addedProductDTO.ImgUrl, "ProductImages");
            var ProductData = new Product
            {
                
                Name = addedProductDTO.Name,
                Description = addedProductDTO.Description,
                ImgUrl = image,
                Price = addedProductDTO.Price,
                Quantity = addedProductDTO.Quantity,
                ProductTypeId = addedProductDTO.ProductTypeId
            };
            unitOfWork.ProductRepository.Add(ProductData);
            return unitOfWork.SaveChanges();
        }

        public  int DeleteProduct(int id)
        {
            var product = unitOfWork.ProductRepository.GetByID(id);
            if (product is not null)
            {
                unitOfWork.ProductRepository.Delete(product);
                return unitOfWork.SaveChanges();
            }
            return 0;
            
        }

        public IEnumerable<ProductDetailsDTO> GetAllProducts()
        {
            List<ProductDetailsDTO> productDetailsDTOs = new List<ProductDetailsDTO>();
            IEnumerable<Product> products = unitOfWork.ProductRepository.GetAll();
            foreach (var product in products)
            {
                var producttype = unitOfWork.ProductTypeRepository.GetByID(product.ProductTypeId);
                productDetailsDTOs.Add(new ProductDetailsDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImgUrl = $"/assets/ProductImages/{product.ImgUrl}",
                    Price = product.Price,
                    Quantity = product.Quantity,
                    ProductTypeName = producttype.Name

                });
            }
            return productDetailsDTOs;
        }

        public ProductDetailsDTO GetProductDetails(int id)
        {
            
            Product productdata = unitOfWork.ProductRepository.GetByID(id);
            if (productdata is null)
                return null;
            var producttype = unitOfWork.ProductTypeRepository.GetByID(productdata.ProductTypeId);
            ProductDetailsDTO productDetailsDTO = new ProductDetailsDTO()
            {
                Id = productdata.Id,
                Name = productdata.Name,
                Description = productdata.Description,
                ImgUrl = $"/assets/ProductImages/{productdata.ImgUrl}",
                Price = productdata.Price,
                Quantity = productdata.Quantity,
                ProductTypeName = producttype.Name

            };
            return productDetailsDTO;
        }

        public async Task<int> UpdateProduct(UpdatedProductDTO updatedProductDTO)
        {
            
            var product = unitOfWork.ProductRepository.GetByID(updatedProductDTO.Id);
            if (product == null)
                return 0;
            var producttype = unitOfWork.ProductTypeRepository.GetByID(product.Id);
            if (updatedProductDTO.ImgUrl != null)
            {
                var fileName = await attachmentService.UploadAsync(updatedProductDTO.ImgUrl, "ProductImages");
                if (!string.IsNullOrEmpty(fileName))
                {
                    product.ImgUrl = fileName;
                }
            }
            product.Name = updatedProductDTO.Name??product.Name;
            product.Description = updatedProductDTO.Description??product.Description;
            product.Price = updatedProductDTO.Price??product.Price;
            product.Quantity = updatedProductDTO.Quantity??product.Quantity;
            product.ProductTypeId = updatedProductDTO.ProductTypeId ?? product.ProductTypeId;

            unitOfWork.ProductRepository.Update(product);
            return unitOfWork.SaveChanges();
        }
    }
}
