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

    public class AppointmentsRepository : GenericRepository<Appointment>, IAppointmentsRepository
    {
        private readonly AppDbContext _context;

        public AppointmentsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Appointment?> GetByGuidAsync(Guid id)
        {
            return await _context.Appointments.FindAsync(id);
        }
    }
}
