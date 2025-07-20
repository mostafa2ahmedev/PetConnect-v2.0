using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.DTO.PetBreadDto;
using PetConnect.BLL.Services.DTO.PetCategoryDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class PetBreadController : ControllerBase
{
    private readonly IPetBreadService _petBreadService;

    public PetBreadController(IPetBreadService petBreadService)
    {
        _petBreadService = petBreadService;
    }

    [HttpGet]
    [EndpointSummary("Get All Breads")]
    [ProducesResponseType(typeof(List<GPetBreadDto>), StatusCodes.Status200OK)]
    public ActionResult GetAll()
    {
        var breads = _petBreadService.GetAllBreads();
        return Ok(new GeneralResponse(200, breads));
    }

    [HttpPost]
    [EndpointSummary("Add A New Bread")]
    public IActionResult Add(AddedPetBreadDto addedPetBreadDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new GeneralResponse(400, "Invalid input data"));

        if (_petBreadService.AddPetBread(addedPetBreadDto) > 0)
            return Ok(new GeneralResponse(200, "Pet bread added successfully"));

        return BadRequest(new GeneralResponse(400, "Failed to add pet bread"));
    }

    [HttpPut]
    [EndpointSummary("Modify An Existing Bread")]
    public IActionResult Edit( UPetBreadDto uPetBreadDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new GeneralResponse(400, "Invalid input data"));

        if (_petBreadService.UpdatePetBread(uPetBreadDto) > 0)
            return Ok(new GeneralResponse(200, "Pet bread updated successfully"));

        return NotFound(new GeneralResponse(404, $"No bread found with ID = {uPetBreadDto.Id}"));
    }

    [HttpDelete]
    [EndpointSummary("Delete An Existing Bread")]
    public IActionResult Delete(int? id)
    {
        if (id==null)
            return BadRequest(new GeneralResponse(400, "Invalid ID"));

        if (_petBreadService.DeletePetBread(id.Value) > 0)
            return Ok(new GeneralResponse(200, "Pet bread deleted successfully"));

        return NotFound(new GeneralResponse(404, $"No bread found with ID = {id}"));
    }
}
