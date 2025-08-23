using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetBreadDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class PetBreedController : ControllerBase
{
    private readonly IPetBreedService _petBreedService;

    public PetBreedController(IPetBreedService petBreedService)
    {
        _petBreedService = petBreedService;
    }

    [HttpGet]
    [EndpointSummary("Get All Breeds")]
    [ProducesResponseType(typeof(List<GPetBreedDto>), StatusCodes.Status200OK)]
    public ActionResult GetAll()
    {
        var breeds = _petBreedService.GetAllBreeds();
        return Ok(new GeneralResponse(200, breeds));
    }

    [HttpGet("{id}")]
    [EndpointSummary("Get Breed By Id")]
    public ActionResult PetBreedDetails(int id)
    {
        //if(id==null)  > GetAll
        var PetBreed= _petBreedService.GetBreedById(id);
        if (PetBreed == null)
            return NotFound(new GeneralResponse(404, $"No Breed found with ID ={id}"));
        return Ok(new GeneralResponse(200, PetBreed));
    }
    [HttpPost]
    [EndpointSummary("Add A New Breed")]
    public IActionResult Add([FromForm] AddedPetBreedDto addedPetBreedDto)
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

        if (_petBreedService.AddPetBread(addedPetBreedDto) > 0)
            return Ok(new GeneralResponse(200, "Pet Breed added successfully"));

        return BadRequest(new GeneralResponse(400, "Failed to add pet Breed"));
    }

    [HttpPut]
    [EndpointSummary("Modify An Existing Breed")]
    public IActionResult Edit([FromForm] UPetBreedDto uPetBreedDto)
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

        if (_petBreedService.UpdatePetBread(uPetBreedDto) > 0)
            return Ok(new GeneralResponse(200, "Pet Breed updated successfully"));

        return NotFound(new GeneralResponse(404, $"No Breed found with ID = {uPetBreedDto.Id}"));
    }

    [HttpDelete]
    [EndpointSummary("Delete An Existing Breed")]
    public IActionResult Delete(int? id)
    {
        if (id == null)
            return BadRequest(new GeneralResponse(400, "Invalid ID"));

        if (_petBreedService.DeletePetBreed(id.Value) == 0)
            return NotFound(new GeneralResponse(404, $"No Breed found with ID = {id}"));

        return Ok(new GeneralResponse(200, "Pet Breed deleted successfully"));
    }

    [HttpGet("Breeds/{id}")]
    [ProducesResponseType(typeof(List<GPetBreedDto>), StatusCodes.Status200OK)]
    [EndpointSummary("Get Breeds By Category")]
    public IActionResult GetBreadsByCategory(int? id)
    {
        if (id == null)
            return BadRequest(new GeneralResponse(400, "Invalid ID"));

        var BreedList = _petBreedService.GetBreedsByCategoryId(id.Value);

        if (BreedList is not { })
            return NotFound(new GeneralResponse(404, $"No Category found with ID = {id}"));

        return Ok(new GeneralResponse(200, BreedList));
    }
}
