using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.DTOs.OrderProduct;
using PetConnect.BLL.Services.DTOs.Seller;
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
    public class SellerService : ISellerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService attachmentService;

        public SellerService(IUnitOfWork unitOfWork,IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            this.attachmentService = attachmentService;
        }
        public  IEnumerable<SellerDataDto> GetAllSellers()
        {
            return _unitOfWork.SellerRepository.GetAll()
          .Select(c => new SellerDataDto
          {
               Id = c.Id,
              FName = c.FName,
              LName = c.LName,
              ImgUrl = c.ImgUrl,
              City = c.Address.City
          }).ToList();
        }

        public SellerDetailsDto? GetProfile(string id)
        {
            var Seller = _unitOfWork.SellerRepository.GetByID(id);

            if (Seller == null)
                return null;

            return new SellerDetailsDto
            {
                FName = Seller.FName,
                LName = Seller.LName,
                ImgUrl = Seller.ImgUrl,
                Gender = Seller.Gender,
                Street = Seller.Address.Street,
                City = Seller.Address.City,
                Country = Seller.Address.Country,
            };
        }

        public int Delete(string id)
        {
            var Seller = _unitOfWork.SellerRepository.GetByID(id);
            if (Seller is not null)
            {
                _unitOfWork.SellerRepository.Delete(Seller);
                return _unitOfWork.SaveChanges();
            }
            return 0;
        }

        public async Task<int> UpdateProfile(string SellerId,UpdateSellerProfileDto sellerProfileDTO)
        {
            var Seller = _unitOfWork.SellerRepository.GetByID(SellerId);
            if (Seller == null)
                return 0;

            if (sellerProfileDTO.ImageFile != null && sellerProfileDTO.ImageFile.Length > 0)
            {
                var fileName = await attachmentService.UploadAsync(sellerProfileDTO.ImageFile, Path.Combine("img", "person"));
                if (!string.IsNullOrEmpty(fileName))
                {
                    Seller.ImgUrl = $"/assets/img/person/{fileName}";
                }
            }

            Seller.FName = sellerProfileDTO.FName;
            Seller.LName = sellerProfileDTO.LName;
            Seller.Gender = sellerProfileDTO.Gender;

          
                Seller.Address = new Address()
                {
                    City = sellerProfileDTO.City,
                    Street = sellerProfileDTO.Street,
                    Country = sellerProfileDTO.Country

                };

            _unitOfWork.SellerRepository.Update(Seller);
            return _unitOfWork.SaveChanges();
        }


        
    }
}
