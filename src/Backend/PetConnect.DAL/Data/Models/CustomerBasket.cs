using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Models
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public ICollection<BasketItem>? Items { get; set; }

        


    }


}
