using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;

namespace PetConnect.DAL.Data.Repositories.Interfaces
{
    public interface IUserConnectionRepository : IGenericRepository<UserConnection>
    {
    }
}
