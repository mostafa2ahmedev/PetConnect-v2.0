using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO
{
    public class UpdatedPetDto
    {
        public string Name { get; set; } 
        public PetStatus Status { get; set; }
        public bool IsApproved { get; set; }
        public Ownership Ownership { get; set; }
        public string ImgUrl { get; set; } 



        public int BreedId { get; set; }
    }
}
