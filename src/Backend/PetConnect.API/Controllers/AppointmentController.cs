using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.BLL.Services.DTOs.AppointmentDto;
using System;
using System.Threading.Tasks;

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
                return NotFound("Appointment not found or already canceled.");

            return Ok("Appointment canceled successfully.");
        }
    }
}

