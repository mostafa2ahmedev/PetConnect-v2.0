using PetConnect.BLL.Services.DTO.PetBreadDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IPetBreadService
    {
        public List<GPetBreadDto> GetAllBreads();
        public UPetBreadDto? GetBreadById(int Id);
        public int AddPetBread(AddedPetBreadDto AddedPetBread);
        public int UpdatePetBread(UPetBreadDto UPetBread);
        public int DeletePetBread(int Id);

        public List<GPetBreadDto> GetBreadsByCategoryId(int CategoryId);
    }
}
