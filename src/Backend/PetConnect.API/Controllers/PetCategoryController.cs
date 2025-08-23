using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetCategoryDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class PetCategoryController : ControllerBase
{
    private readonly IPetCategoryService _petCategoryService;

    public PetCategoryController(IPetCategoryService petCategoryService)
    {
        _petCategoryService = petCategoryService;
    }

    [HttpGet]
    [EndpointSummary("Get All Categories")]
    [ProducesResponseType(typeof(List<GPetCategoryDto>), StatusCodes.Status200OK)]
    public ActionResult GetAll()
    {
        var categories = _petCategoryService.GetAllCategories();
        return Ok(new GeneralResponse(200, categories));
    }

    [HttpGet("{id}")]
    [EndpointSummary("Get Category By Id")]
    public ActionResult PetCategoryDetails(int id)
    {
        //if(id==null)  > GetAll
        var PetCategory = _petCategoryService.GetPetCategoryById(id);
        if (PetCategory == null)
            return NotFound(new GeneralResponse(404, $"No Category found with ID ={id}"));
        return Ok(new GeneralResponse(200, PetCategory));
    }

    [HttpPost]
    [EndpointSummary("Add A New Category")]
    public IActionResult Add([FromForm] AddedPetCategoryDTO addedPetCategoryDto)
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

        if (_petCategoryService.AddPetCategory(addedPetCategoryDto) > 0)
            return Ok(new GeneralResponse(200, "Pet category added successfully"));

        return BadRequest(new GeneralResponse(400, "Failed to add pet category"));
    }

    [HttpPut]
    [EndpointSummary("Modify An Existing Category")]
    public IActionResult Edit([FromForm] UPetCategoryDto uPetCategoryDto)
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

        if (_petCategoryService.UpdatePetCategory(uPetCategoryDto) > 0)
            return Ok(new GeneralResponse(200, "Pet category updated successfully"));

        return NotFound(new GeneralResponse(404, $"No category found with ID = {uPetCategoryDto.Id}"));
    }

    [HttpDelete]
    [EndpointSummary("Delete An Existing Category")]
    public IActionResult Delete(int? id)
    {
        if (id == null)
            return BadRequest(new GeneralResponse(400, "Invalid ID"));

        if (_petCategoryService.DeletePetCategory(id.Value) == 0)
            return NotFound(new GeneralResponse(404, $"No category found with ID = {id}"));

        return Ok(new GeneralResponse(200, "Pet category deleted successfully"));
    }
}
