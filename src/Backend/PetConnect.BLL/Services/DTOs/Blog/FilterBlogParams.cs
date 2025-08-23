using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.Blog
{
    public class FilterBlogParams
    {
        public int? Topic { get; set; } = null;
        public List<int>? Categoies_ID { get; set; } = null;
    }
}
