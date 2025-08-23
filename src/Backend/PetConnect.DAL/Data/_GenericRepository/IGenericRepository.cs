using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Repositories;

namespace PetConnect.DAL.Data.GenericRepository
{
    public interface IGenericRepository<T>  // T > Entity
    {
        public IEnumerable<T> GetAll(bool withracking = false); // Get All T
        public IQueryable<T> GetAllQueryable(bool withracking = false); // Get All T
        public T? GetByID(int id); // Get Element By Id

        public T? GetByID(string id); // Get Element By Id
        public T? GetByID(Guid Id);
        public void Add(T entity); // Add entity record to DbSet

        public void Update(T entity); // update entity data from existing DbSet

        public void Delete(T entity); // delete entity data from existing DbSet


    }
}
