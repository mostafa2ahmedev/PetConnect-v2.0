using Microsoft.AspNetCore.Http;
using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.PetDto
{
    public class UpdatedPetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public PetStatus Status { get; set; }
        public bool IsApproved { get; set; }
        public Ownership Ownership { get; set; }
        public IFormFile form { get; set; } = null!;
        public int BreedId { get; set; }
    }
}
