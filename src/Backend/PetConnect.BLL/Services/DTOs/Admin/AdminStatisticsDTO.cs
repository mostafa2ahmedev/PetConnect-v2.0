using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Admin
{
    public class AdminStatisticsDTO
    {
        public int TotalUsers { get; set; }
        public int TotalDoctors { get; set; }
        public int ApprovedDoctors { get; set; }
        public int RejectedDoctors { get; set; }
        public int PendingDoctors { get; set; }

        public int TotalCustomers { get; set; }
        public int TotalPets { get; set; }
        public int ApprovedPets { get; set; }
        public int RejectedPets { get; set; }
        public int PendingPets { get; set; }

        public int PetsForAdoption { get; set; }
        public int PetsForRescue { get; set; }
    }
}
