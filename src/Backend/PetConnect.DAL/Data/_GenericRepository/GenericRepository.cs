using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetConnect.DAL.Data.Models;

namespace PetConnect.DAL.Data.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext context;

        public GenericRepository(AppDbContext _context)
        {
            context = _context;
        }
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }
        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
           
        }
        public IQueryable<T> GetAllQueryable(bool withracking = false)
        {
            if (withracking)
                return context.Set<T>();
            else
                return context.Set<T>().AsNoTracking();
        }
        public IEnumerable<T> GetAll(bool withracking = false)
        {
            if (withracking)
                return context.Set<T>().ToList();
            else
               return context.Set<T>().AsNoTracking().ToList();     

        }



        public T? GetByID(int id)
        {
            return context.Set<T>().Find(id);
        }
        public T? GetByID(string id)
        {
            return context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }

        


    }
}
