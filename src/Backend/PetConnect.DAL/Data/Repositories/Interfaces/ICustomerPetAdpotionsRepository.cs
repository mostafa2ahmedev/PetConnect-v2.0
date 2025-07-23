using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Interfaces;
    public interface ICustomerPetAdoptionsRepository : IGenericRepository<CustomerPetAdoptions>
    {

    public CustomerPetAdoptions? GetCustomerAdoptionRecord(string UserId,string RecCustomerId,int PetId);
    }

