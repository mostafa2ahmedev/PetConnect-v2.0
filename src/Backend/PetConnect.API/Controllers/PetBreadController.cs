using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetBreadDto;
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

    [HttpGet("{id}")]
    [EndpointSummary("Get Bread By Id")]
    public ActionResult PetBreadDetails(int id)
    {
        //if(id==null)  > GetAll
        var PetBread= _petBreadService.GetBreadById(id);
        if (PetBread == null)
            return NotFound(new GeneralResponse(404, $"No BREAD found with ID ={id}"));
        return Ok(new GeneralResponse(200, PetBread));
    }
    [HttpPost]
    [EndpointSummary("Add A New Bread")]
    public IActionResult Add([FromForm] AddedPetBreadDto addedPetBreadDto)
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

        if (_petBreadService.AddPetBread(addedPetBreadDto) > 0)
            return Ok(new GeneralResponse(200, "Pet bread added successfully"));

        return BadRequest(new GeneralResponse(400, "Failed to add pet bread"));
    }

    [HttpPut]
    [EndpointSummary("Modify An Existing Bread")]
    public IActionResult Edit([FromForm] UPetBreadDto uPetBreadDto)
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

        if (_petBreadService.UpdatePetBread(uPetBreadDto) > 0)
            return Ok(new GeneralResponse(200, "Pet bread updated successfully"));

        return NotFound(new GeneralResponse(404, $"No bread found with ID = {uPetBreadDto.Id}"));
    }

    [HttpDelete]
    [EndpointSummary("Delete An Existing Bread")]
    public IActionResult Delete(int? id)
    {
        if (id == null)
            return BadRequest(new GeneralResponse(400, "Invalid ID"));

        if (_petBreadService.DeletePetBread(id.Value) == 0)
            return NotFound(new GeneralResponse(404, $"No bread found with ID = {id}"));

        return Ok(new GeneralResponse(200, "Pet bread deleted successfully"));
    }

    [HttpGet("Breads/{id}")]
    [ProducesResponseType(typeof(List<GPetBreadDto>), StatusCodes.Status200OK)]
    [EndpointSummary("Get Breads By Category")]
    public IActionResult GetBreadsByCategory(int? id)
    {
        if (id == null)
            return BadRequest(new GeneralResponse(400, "Invalid ID"));

        var BreadList = _petBreadService.GetBreadsByCategoryId(id.Value);

        if (BreadList is not { })
            return NotFound(new GeneralResponse(404, $"No Category found with ID = {id}"));

        return Ok(new GeneralResponse(200, BreadList));
    }
}
