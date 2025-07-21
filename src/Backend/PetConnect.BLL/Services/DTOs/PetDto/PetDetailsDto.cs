using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.PetDto
{
    public class PetDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
 
        public int? Age { get; set; }
        public PetStatus Status { get; set; }
        public bool IsApproved { get; set; }
        public Ownership Ownership { get; set; }
        public string ImgUrl { get; set; } = null!;

        public string BreadName { get; set; } = null!;

        public string CategoryName { get; set; } = null!;
    }
}
