using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Enums
{
    public enum AppointmentStatus
    {
        Confirmed,   // Default: auto-approved and upcoming
        Completed,   // Marked when appointment has successfully happened
        Cancelled,    // Manually cancelled by customer or system
        Pending         //customer waiting for doctors confirmation
    }
}
