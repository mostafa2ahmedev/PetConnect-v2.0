using PetConnect.DAL.Data.GenericRepository;
using PetConnect.DAL.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using PetConnect.DAL.Data.GenericRepository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetConnect.DAL.Data.Models;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    class TimeSlotsRepository : GenericRepository<TimeSlot>, ITimeSlotsRepository
    {
        private readonly AppDbContext context;

        public TimeSlotsRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
