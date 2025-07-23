using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs;
using PetConnect.BLL.Services.DTOs.TimeSlotDto;
using PetConnect.BLL.Services.Interfaces;
namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotsController : ControllerBase
    {
        private readonly ITimeSlotService _timeSlotService;
        public TimeSlotsController(ITimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
        }
        [HttpGet("{doctorId}")]
        [ProducesResponseType(typeof(List<DataTimeSlotsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Get All Active TimeSlots")]
        public IActionResult GetAllActiveTimeSlots(string doctorId)
        {
            if (doctorId == null)
                return BadRequest(new GeneralResponse(400, "Invalid didn't find any doctor with the following id"));
            var timeSlots = _timeSlotService.GetAllActiveTimeSlots(doctorId);
            if (timeSlots != null && timeSlots.Count() >= 1)
                return Ok(new GeneralResponse(200, timeSlots));
            return NotFound(new GeneralResponse(404, timeSlots));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [EndpointSummary("Add TimeSlot ")]
        public IActionResult AddTimeSlot(AddedTimeSlotDto addedTimeSlot)
        {
            if (addedTimeSlot == null)
                return BadRequest(new GeneralResponse(400, "cannot add empty time slot"));
            var response = _timeSlotService.AddTimeSlot(addedTimeSlot);
            return Ok(new GeneralResponse(200, response));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [EndpointSummary("Delete TimeSlot ")]
        public IActionResult DeleteTimeSlot(int id)
        {
            if (id == null)
                return BadRequest(new GeneralResponse(400, "Invalid Id"));

            if (_timeSlotService.DeleteTimeSlot(id) == 0)
            {
                return NotFound(new GeneralResponse(404, $"No Timeslot found with ID = {id}"));
            }

            return Ok(new GeneralResponse(200, "Timeslot deleted successfully"));


        }

        [HttpPut]
        [EndpointSummary("Modify An Existing Timeslot")]
        public async Task<ActionResult> Edit([FromForm] UpdatedTimeSlotDto updatedTimeSlot)
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

            var result = await _timeSlotService.UpdateTimeSlot(updatedTimeSlot);
            if (result == 0)
                return NotFound(new GeneralResponse(404, $"No Time slot found with ID = {updatedTimeSlot.Id}"));

            return Ok(new GeneralResponse(200, "Time slot updated successfully"));
        }
    }
}
