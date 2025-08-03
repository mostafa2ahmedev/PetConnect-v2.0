using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.BLL.Services.DTOs.AppointmentDto;
using System;
using System.Threading.Tasks;
using PetConnect.BLL.Services.DTOs;

namespace PetConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        // GET: api/Appointment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
                return NotFound("Appointment not found.");
            return Ok(appointment);
        }

        // GET: api/Appointment/Customer/{customerId}
        [HttpGet("Customer/{customerId}")]
        public async Task<IActionResult> GetAppointmentsByCustomer(string customerId)
        {
            var appointments = await _appointmentService.GetAppointmentsByCustomerAsync(customerId);
            return Ok(appointments);
        }

        // GET: api/Appointment/Doctor/{doctorId}
        [HttpGet("Doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(string doctorId)
        {
            var appointments = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId);
            return Ok(appointments);
        }

        // POST: api/Appointment
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdAppointment = await _appointmentService.AddAppointmentAsync(dto);

            if (createdAppointment != null)
                return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.Id }, createdAppointment);

            return BadRequest("Failed to create appointment.");
        }

        // PUT: api/Appointment/{id}/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(Guid id)
        {
            var result = await _appointmentService.CancelAppointmentAsync(id);
            if (!result)
                return BadRequest(new GeneralResponse(400, "Appointment not found or already Canceled."));

            return Ok(new GeneralResponse(200, "Appointment Canceled successfully."));
        }
        // PUT: api/Appointment/{id}/confirm
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmAppointment(Guid id)
        {
            var result = await _appointmentService.ConfirmAppointmentAsync(id);
            if (!result)
                return BadRequest(new GeneralResponse(400, "Appointment not found or already Confirmed."));

            return Ok(new GeneralResponse(200, "Appointment Confirmed successfully."));
        }

        // PUT: api/Appointment/{id}/book
        [HttpPut("{id}/book")]
        public async Task<IActionResult> BookAppointment(Guid id)
        {
            var result = await _appointmentService.BookAppointmentAsync(id);
            if (!result)
                return BadRequest(new GeneralResponse(400, "Cannot Book at this timeslot"));

            return Ok(new GeneralResponse(200, "Appointment Booked successfully."));
        }

        // PUT: api/Appointment/{id}/complete
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var result = await _appointmentService.CompleteAppointmentAsync(id);
            if (!result)
                return BadRequest(new GeneralResponse(400,"Appointment not found or already Completed."));

            return Ok(new GeneralResponse(200, "Appointment Completed successfully."));
        }

        // GET: api/Appointment/DoctorProfile/{doctorId}
        [HttpGet("DoctorProfile/{doctorId}")]
        public IActionResult GetAppointmentsForDoctorProfile(string doctorId)
        {
            var appointments = _appointmentService.GetAppointmentsForDoctorProfile(doctorId);
            if (appointments == null)
                return NotFound();
            return Ok(appointments);
        }

        // Post: api/Appointment/Book
        [HttpPost("Book")]
        public IActionResult GetAppointmentsForCustomerBookingDoctor(string doctorId,string customerId)
        {
            var appointments = _appointmentService.GetCurrentTimeSlotsAvailableDocCustomer(doctorId,customerId);
            if (appointments == null)
                return NotFound();
            return Ok(appointments);
        }

    }
}

