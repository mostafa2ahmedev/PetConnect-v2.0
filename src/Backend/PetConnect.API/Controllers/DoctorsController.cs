using Microsoft.AspNetCore.Mvc;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.DAL.Data.Enums;
using PetConnect.BLL.Common.AttachmentServices;

namespace PetConnect.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService doctorService;
        private readonly IAttachmentService attachmentService;

        public DoctorsController(IDoctorService _doctorService, IAttachmentService _attachmentService)
        {
            doctorService = _doctorService;
            attachmentService = _attachmentService;
        }

        // GET: api/doctors
        [HttpGet]
        public IActionResult GetAll(string? name, decimal? maxPrice, PetSpecialty? specialty)
        {
            var doctors = doctorService.GetAll();

            if (!string.IsNullOrEmpty(name))
            {
                doctors = doctors
                    .Where(d => d.FName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                                d.LName.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (maxPrice.HasValue)
            {
                doctors = doctors.Where(d => d.PricePerHour <= maxPrice.Value);
            }

            if (specialty.HasValue)
            {
                doctors = doctors.Where(d => d.PetSpecialty == specialty.Value.ToString());
            }

            var filteredList = doctors.ToList();

            if (!filteredList.Any())
                return NotFound("No doctors found matching the criteria.");

            return Ok(filteredList);
        }

        // GET: api/doctors/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id is required.");

            var doctorDTO = doctorService.GetByID(id);

            if (doctorDTO == null)
                return NotFound();

            return Ok(doctorDTO);
        }

        // PUT: api/doctors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromForm] DoctorDetailsDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingDoctor = doctorService.GetByID(dto.Id);
                if (existingDoctor == null)
                    return NotFound();

                if (dto.ImageFile != null)
                {
                    var fileName = await attachmentService.UploadAsync(dto.ImageFile, Path.Combine("img", "doctors"));
                    if (fileName != null)
                    {
                        dto.ImgUrl = $"/assets/img/doctors/{fileName}";
                    }
                    else
                    {
                        return BadRequest("Invalid image file. Please upload a PNG, JPG, JPEG or PDF under 2MB.");
                    }
                }
                else
                {
                    dto.ImgUrl = existingDoctor.ImgUrl;
                }

                if (dto.CertificateFile != null)
                {
                    var fileName = await attachmentService.UploadAsync(dto.CertificateFile, Path.Combine("img", "certificates"));
                    if (fileName != null)
                    {
                        dto.CertificateUrl = $"/assets/img/certificates/{fileName}";
                    }
                    else
                    {
                        return BadRequest("Invalid certificate file. Please upload a PNG, JPG, JPEG or PDF under 2MB.");
                    }
                }
                else
                {
                    dto.CertificateUrl = existingDoctor.CertificateUrl;
                }

                doctorService.Update(dto);

                return Ok("Doctor updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Update failed: {ex.Message}");
            }
        }
    }
}
