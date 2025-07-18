using PetConnect.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTO.PetDto
{
    public class PetDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public PetStatus Status { get; set; }
        public string ImgUrl { get; set; } = null!;

        public string CategoryName { get; set; } = null!;
    }
}
