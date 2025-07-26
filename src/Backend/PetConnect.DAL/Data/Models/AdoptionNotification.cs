using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class AdoptionNotification 
    {
        public  Guid NotificationId { get; set; }
        public Notification Notification { get; set; } = null!;

        public Customer Customer { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
    }
}
