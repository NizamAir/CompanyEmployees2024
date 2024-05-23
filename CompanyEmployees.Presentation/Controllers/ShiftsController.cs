using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ShiftDTOs;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/shifts")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public ShiftsController(IServiceManager service)
        {
            _service = service;
        }
        /*[HttpPost("doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateShiftDoctor([FromBody] ShiftForDoctorCreationDto shifts)
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            if (shifts.Dates.Count == 0)
            {
                return BadRequest("CompanyForCreationDto object is null");
            }
            var shiftsMy = shifts.Dates.ToArray();
            foreach (var shift in shiftsMy)
            {
                await _service.ShiftService.CreateShiftForDoctor(userId, shift);
            }

            return Ok();
        }*/

        [HttpPost("assistant")]
        [Authorize(Roles = "Assistant")]
        public async Task<IActionResult> UpdateShiftAssistant([FromBody] ShiftForAssistantCreationDto shiftForAssistant)
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            if (shiftForAssistant.Dates.Count == 0)
                return BadRequest("CompanyForCreationDto object is null");

            var shiftsMy = shiftForAssistant.Dates.ToArray();
            foreach (var shift in shiftsMy)
            {
                await _service.ShiftService.UpdateShiftAssistant(userId, shiftForAssistant.DoctorId, shift, trackChanges: true);
            }
            return Ok();
        }

        /*[HttpPost("user")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateShiftClient([FromBody] ShiftForClientCreationDto shiftForClient)
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            if (shiftForClient.Date.Equals(null))
                return BadRequest("CompanyForCreationDto object is null");

            await _service.ShiftService.UpdateShiftClient(userId, shiftForClient.DoctorId, shiftForClient.Date, trackChanges: true);
            
            return Ok();
        }*/


        [HttpGet]
        public async Task<IActionResult> GetShifts()
        {
            var shifts = await _service.ShiftService.GetAllShifts(trackChanges: false);
            return Ok(shifts);
        }

        /*[HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetShiftsByDoctor(string doctorId)
        {
            var shifts = await _service.ShiftService.GetShiftsByDoctor(doctorId, trackChanges: false);
            return Ok(shifts);
        }*/

        [HttpGet("{id:guid}", Name = "ShiftById")]
        public async Task<IActionResult> GetShift(Guid id)
        {
            var shift = await _service.ShiftService.GetShift(id, trackChanges: false);
            return Ok(shift);
        }
    }
}
