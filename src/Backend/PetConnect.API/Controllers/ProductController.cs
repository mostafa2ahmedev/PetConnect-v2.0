using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Product;
using PetConnect.BLL.Services.Interfaces;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService _ProductService)
        {
            productService = _ProductService;
        }
        [HttpGet()]
        [ProducesResponseType(typeof(List<ProductDetailsDTO>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Products")]
        public IActionResult GetAllProducts()
        {
            var products = productService.GetAllProducts();
            return Ok(products);
        }
        [HttpGet("{id}")]
        [EndpointSummary("Get Product By Id")]
        public IActionResult GetProduct(int id) 
        {
            var product = productService.GetProductDetails(id);
            return Ok(product);
        }
        [HttpPost]
        [EndpointSummary("Add A New Product")]
        public async Task<ActionResult> AddProduct([FromForm] AddedProductDTO AddProduct)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new GeneralResponse(400, errors));
            }


            await productService.AddProduct(AddProduct);
            return Ok(new GeneralResponse(200, "Product added successfully"));
        }

    }
}
