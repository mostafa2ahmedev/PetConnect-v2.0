using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Interfaces;
using System.IO; 
using System.Threading.Tasks;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceComparisonController : ControllerBase
    {
        private readonly IFaceComparisonService _faceCompareService;

        public FaceComparisonController(IFaceComparisonService faceCompareService)
        {
            _faceCompareService = faceCompareService;
        }

        [HttpPost]
        public async Task<IActionResult> Compare(IFormFile image1, IFormFile image2)
        {
            if (image1 == null || image2 == null)
            {
                return BadRequest(new { facesMatch = false, message = "Please upload two images." });
            }

            var areMatching = await _faceCompareService.AreFacesMatchingAsync(
                image1.OpenReadStream(),
                image2.OpenReadStream()
            );

            return Ok(new { facesMatch = areMatching });

        }
    }
}
