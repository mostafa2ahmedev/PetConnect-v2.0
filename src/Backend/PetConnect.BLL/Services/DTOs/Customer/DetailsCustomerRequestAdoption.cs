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
        
        public DateTime AdoptionDate { get; set; }
        public AdoptionStatus Status { get; set; }

        public string PetName { get; set; } = null!;
        public string PetBreadName { get; set; } = null!;
        public string PetCategoryName { get; set; } = null!;
    }
}
