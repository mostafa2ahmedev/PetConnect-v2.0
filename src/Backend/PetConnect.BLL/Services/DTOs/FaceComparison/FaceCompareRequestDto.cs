using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs.FaceComparison
{
    public class FaceCompareRequestDto
    {
        public IFormFile Image1 { get; set; }
        public IFormFile Image2 { get; set; }
    }
}
