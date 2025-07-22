using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PetConnect.API.Controllers
{
    [Authorize(Roles ="Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test() 
        {
            return Content("tokkkekkekekkekeken");
        }
    }
}
