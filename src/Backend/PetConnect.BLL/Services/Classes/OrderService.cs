using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.DTOs.Order;
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
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentService attachmentService;
        public OrderService(IUnitOfWork _unitOfWork , IAttachmentService _attachmentService)
        {
            unitOfWork = _unitOfWork;
            attachmentService = _attachmentService;
        }

        public Task<int> AddOrder(AddedOrderDTO addedOrderDTO)
        {
            throw new NotImplementedException();

        }

        public int DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderDetailsDTO> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public OrderDetailsDTO GetOrderDetails(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateOrder(UpdatedOrderDTO updatedOrderDTO)
        {
            throw new NotImplementedException();
        }
    }
}
