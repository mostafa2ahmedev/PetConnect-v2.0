using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Support.Admin
{
    public class UpdatePriorityStatusDto
    {
        [Required]
        public int SupportRequestId { get; set; }

        [Required]
        public SupportRequestPriority Priority { get; set; }
    }
}
