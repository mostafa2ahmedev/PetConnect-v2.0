using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;

using PetConnect.BLL.Services.DTO.PetCategoryDto;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System.Security.Claims;

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

        #region Get All

        [HttpGet()]
        [ProducesResponseType(typeof(List<PetDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All Pets")]
        public ActionResult GetAll()
        {
            var pets = _petService.GetAllApprovedPetsWithCustomerData();
            return Ok(new GeneralResponse(200, pets));
        }

        #endregion


        #region GetAll "For Adoption Pets"
        [HttpGet("ForAdoptions")]
        [ProducesResponseType(typeof(List<PetDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All For Adoption Pets")]
        public ActionResult GetAllForAdoptionPets()
        {
            var pets = _petService.GetAllForAdoptionPetsWithCustomerData();
            return Ok(new GeneralResponse(200, pets));
        }
        #endregion

        #region GetAll "For Rescue Pets"
        [HttpGet("ForRescue")]
        [ProducesResponseType(typeof(List<PetDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get All For Rescue Pets")]
        public ActionResult GetAllForRescuePets()
        {
            var pets = _petService.GetAllForRescuePetsWithCustomerData();
            return Ok(new GeneralResponse(200, pets));
        }
        #endregion

        #region Get All By Count For Adoption
        [HttpGet("Count/{count}")]
        [ProducesResponseType(typeof(List<PetDataDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Get Pets with Limit For Adoption")]

        public ActionResult GetAllPetByCountForAdoption(int? count)
        {
            if (count == null)
                return BadRequest(new GeneralResponse(400, "Count Can't be null"));


            var pets = _petService.GetAllPetsByCountForAdoption(count.Value);

            return Ok(new GeneralResponse(200, pets));
        }
        #endregion

        #region Get By ID

        [HttpGet("{id}")]
        [EndpointSummary("Get Pet By Id")]
        public ActionResult PetDetails(int id)
        {  
            //if(id==null)  > GetAll
            var Pet = _petService.GetPet(id);
            if (Pet == null)
                return NotFound(new GeneralResponse(404, $"No Pet found with ID ={id}"));
            return Ok(new GeneralResponse(200, Pet));
        }


        #endregion


        #region Add Pet
        [HttpPost()]
        [EndpointSummary("Add A New Pet")]
        [Authorize(Roles ="Customer")]
        public async Task<ActionResult> AddPet([FromForm] AddedPetDto addPet)
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

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            await _petService.AddPet(addPet,customerId!);

            return Ok(new GeneralResponse(200, "Pet added successfully"));
        }

        #endregion

        #region Edit Pet
        [HttpPut]
        [EndpointSummary("Modify An Existing Pet")]
        public async Task<ActionResult> Edit( [FromForm]UpdatedPetDto UpdatedPetDto)
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

            var result = await _petService.UpdatePet(UpdatedPetDto);
            if (result == 0)
                return NotFound(new GeneralResponse(404, $"No pet found with ID = {UpdatedPetDto.Id}"));

            return Ok(new GeneralResponse(200, "Pet updated successfully"));


        }
        #endregion

        #region Delete Pet
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
        #endregion


        #region Get Pet By Customer ID

        [HttpGet("Customer/{CustomerId}")]
        [EndpointSummary("Get Pets By Customer Id")]
        public ActionResult PetsForCustomer(string CustomerId)
        {
            //if(id==null)  > GetAll
            var pets = _petService.GetPetsForCustomer(CustomerId);
            if (pets == null || pets.Count()==0)
                return NotFound(new GeneralResponse(404, $"No Pets found with this Customer ID"));
            return Ok(new GeneralResponse(200, pets));
        }

        #endregion
    }
}
