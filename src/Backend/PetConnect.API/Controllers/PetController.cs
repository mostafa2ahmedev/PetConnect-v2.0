using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;

using PetConnect.BLL.Services.DTO.PetCategoryDto;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }


        [HttpGet()]
        [ProducesResponseType(typeof(List<PetDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Pets")]
        public ActionResult GetAll()
        {
            var pets = _petService.GetAllPets();
            return Ok(new GeneralResponse(200, pets));
        }
        [HttpGet("{id}")]
        [EndpointSummary("Get Pet By Id")]
        public ActionResult PetDetails(int? id)
        {
            if (id == null)
                return BadRequest(new GeneralResponse(400, "Invalid Id"));
            var Pet = _petService.GetPet(id.Value);
            if (Pet == null)
                return NotFound(new GeneralResponse(404, $"No Pet found with ID ={id}"));
            return Ok(new GeneralResponse(200, Pet));
        }
        [HttpPost()]
        [EndpointSummary("Add A New Pet")]
        public async Task<ActionResult> AddPet( AddedPetDto addPet)
        {
            if (!ModelState.IsValid)
                return BadRequest(new GeneralResponse(400, "Invalid input data"));

            await _petService.AddPet(addPet);
            return Ok(new GeneralResponse(200, "Pet added successfully"));
        }



        [HttpPut]
        [EndpointSummary("Modify An Existing Pet")]
        public  async Task<ActionResult> Edit(UpdatedPetDto UpdatedPetDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new GeneralResponse(400, "Invalid data"));

            var result = await _petService.UpdatePet(UpdatedPetDto);
            if (result == 0)
                return NotFound(new GeneralResponse(404, $"No pet found with ID = {UpdatedPetDto.Id}"));

            return Ok(new GeneralResponse(200, "Pet updated successfully"));
            

        }

        [HttpDelete]
        [EndpointSummary("Delete An Existing Pet")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest(new GeneralResponse(400, "Invalid Id"));
           
            if (_petService.DeletePet(id.Value) == 0)
            {
                return NotFound(new GeneralResponse(404, $"No pet found with ID = {id}"));
            }

            return Ok(new GeneralResponse(200, "Pet deleted successfully"));



        }
    }
}
