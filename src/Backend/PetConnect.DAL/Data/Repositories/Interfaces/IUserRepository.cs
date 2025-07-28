using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
    }
}
