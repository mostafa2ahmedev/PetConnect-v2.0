using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.DTOs
{
    public class GeneralResponse
    {
        public int StatusCode { get; set; }
        public dynamic? Data { get; set; }
        public GeneralResponse(int statusCode, dynamic? data = null)
        {
            StatusCode = statusCode;
            Data = data;
        }
    }
}
