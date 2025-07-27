using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Customer
{
    public class DetailsCustomerRequestAdoption
    {
        public int PetId { get; set; }
        public DateTime AdoptionDate { get; set; }
        public string AdoptionStatus { get; set; } = null!;
        public string RecCustomerId { get; set; } = null!;
        public string PetName { get; set; } = null!;
        public string PetBreadName { get; set; } = null!;
        public string PetCategoryName { get; set; } = null!;

    }
}
