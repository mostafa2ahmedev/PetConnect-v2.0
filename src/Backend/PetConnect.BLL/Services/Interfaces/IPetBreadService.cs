using PetConnect.BLL.Services.DTO.PetBreadDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IPetBreedService
    {
        public List<GPetBreedDto> GetAllBreeds();
        public GPetBreedDto? GetBreedById(int Id);
        public int AddPetBread(AddedPetBreedDto AddedPetBread);
        public int UpdatePetBread(UPetBreedDto UPetBread);
        public int DeletePetBreed(int Id);

        public List<GPetBreedDto>? GetBreedsByCategoryId(int CategoryId);
    }
}
