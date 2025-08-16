using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class DeliveryMethodRepository : GenericRepository<DeliveryMethod>,IDeliveryMethodRepository
    {
        private readonly AppDbContext _context;

        public DeliveryMethodRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<DeliveryMethod> GetAllDeliveryMethods()
        {
            return _context.Set<DeliveryMethod>().ToList();
        }
    }
}
