using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Product;
using PetConnect.BLL.Services.Interfaces;
using System.Security.Claims;

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
        #region GetAllProducts
        [HttpGet]
        [ProducesResponseType(typeof(List<ProductDetailsDTO>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Products")]
        public IActionResult GetAllProducts()
        {
            var products = productService.GetAllProducts();
            return Ok(products);
        }
        #endregion
        #region GetDetails
        [HttpGet("{id}")]
        [EndpointSummary("Get Product By Id")]
        public IActionResult GetProduct(int id) 
        {
            var product = productService.GetProductDetails(id);
            return Ok(product);
        }
        #endregion
        #region AddProduct
        [HttpPost]
        [EndpointSummary("Add A New Product")]
        [Authorize(Roles = "Seller")]
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

            var SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await productService.AddProduct(SellerId!, AddProduct);
            return Ok(new GeneralResponse(200, "Product added successfully"));
        }
        #endregion
        #region UpdateProduct
        [HttpPut]
        [EndpointSummary("Update Product")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> Edit([FromForm] UpdatedProductDTO updatedProductDTO)
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
            var SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await productService.UpdateProduct(SellerId!, updatedProductDTO);
            if (result == 0)
                return NotFound(new GeneralResponse(404, $"No Product found with ID = {updatedProductDTO.Id}"));

            return Ok(new GeneralResponse(200, "Product updated successfully"));

        }
        #endregion
        #region DeleteProduct
        [HttpDelete]
        [EndpointSummary("Delete Product")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest(new GeneralResponse(400, "Invalid Id"));
            if (productService.DeleteProduct(id.Value) == 0)
            {
                return NotFound(new GeneralResponse(404, $"No Product found with ID = {id}"));
            }
            return Ok(new GeneralResponse(200, "Product deleted successfully"));
        }
        #endregion
    }
}
