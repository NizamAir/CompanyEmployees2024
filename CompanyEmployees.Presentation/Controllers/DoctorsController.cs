using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.DoctorDTOs;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public DoctorsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _service.DoctorService.GetAllDoctors(trackChanges: false);
            return Ok(doctors);
        }

        [HttpGet("{id:guid}", Name = "DoctorById")]
        public async Task<IActionResult> GetDoctor(Guid id)
        {
            var doctor = await _service.DoctorService.GetDoctor(id, trackChanges: false);
            return Ok(doctor);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            await _service.DoctorService.DeleteDoctor(id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDoctor(Guid id, [FromForm] DoctorForUpdateDto doctor)
        {
            if (doctor is null)
                return BadRequest("DoctorForUpdateDto object is null");

            await _service.DoctorService.UpdateDoctor(id, doctor, trackChanges: true);
            return NoContent();
        }
    }
}
