using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface IPetRepository:IGenericRepository<Pet>
    {
        public IQueryable<Pet> GetPendingPetsWithBreedAndCategory();
        public IQueryable<Pet> GetPetDataWithCustomer();
        public Pet GetPetDetails(int id);
    }
}
