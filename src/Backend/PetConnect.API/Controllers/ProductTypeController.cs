using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.Product;
using PetConnect.BLL.Services.DTOs.ProductType;
using PetConnect.BLL.Services.Interfaces;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;
        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }
        #region GetAllProductTypes
        [HttpGet]
        [EndpointSummary("Get All Product types")]
        [ProducesResponseType(typeof(List<ProductTypeDataDTO>), StatusCodes.Status200OK)]
        public IActionResult GetAllProductsTypes()
        {
            var ProductTypes = _productTypeService.GetAllProductsType();
            return Ok(ProductTypes);
        }
        #endregion
        #region ProducTypeDetails
        [HttpGet("{id}")]
        [EndpointSummary("Get Product Type Details")]
        public IActionResult GetProductTypeDetails(int id)
        {
            if (id == null)
                return BadRequest(new GeneralResponse(404, "Id Not Found"));
            var productType = _productTypeService.GetProductTypeDetails(id);
            return Ok(productType);
        }
        #endregion
        #region AddProductType
        [HttpPost]
        [EndpointSummary("Add New Product type")]
        public async Task<ActionResult> Add([FromForm] AddedProductTypeDTO addedProductTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key,
                     kvp => kvp.Value.Errors.Select(s => s.ErrorMessage).ToArray());
                return BadRequest(new GeneralResponse(400 , errors));
            }
            await _productTypeService.AddProductType(addedProductTypeDTO);
            return Ok(new GeneralResponse(200 , "ProductType Added Successfully"));
        }
        #endregion
        #region UpdateProductType
        [HttpPut]
        [EndpointSummary("Update Product Type")]
        public async Task<ActionResult> Edit([FromForm]UpdatedProductTypeDTO updatedProductTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                var Errors = ModelState.Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                return BadRequest(new GeneralResponse(400, Errors));
            }
            var Result = await _productTypeService.UpdateProductType(updatedProductTypeDTO);
            if (Result == 0)
                return NotFound(new GeneralResponse(404, $"No ProductType found with ID = {updatedProductTypeDTO.Id}"));

            return Ok(new GeneralResponse(200, "Product Type updated successfully"));
        }
        #endregion
        #region DeleteProductType
        [HttpDelete]
        [EndpointSummary("Delete Product Type")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest(new GeneralResponse(400, "Invalid Id"));
            if (_productTypeService.DeleteProductType(id.Value) == 0)
            {
                return NotFound(new GeneralResponse(404, $"No Product Type found with ID = {id}"));
            }
            return Ok(new GeneralResponse(200, "Product Type deleted successfully"));
        }
        #endregion
    }
}
