using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Review
{
    public class ReviewByCustomerDTO
    {
        public string DoctorFullname { get; set; }
        public string DocImgURL { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public int ReviewId { get; set; }
    }
}
