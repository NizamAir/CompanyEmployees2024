using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ShiftDTOs;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/doctorshifts")]
    [ApiController]
    public class DoctorShiftsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public DoctorShiftsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAllDoctor()
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            var res = await _service.ShiftService.GetShiftsByDoctor(userId, trackChanges: false);

            return Ok(res);

        }

        [HttpGet("dates")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorDays()
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            var res = await _service.ShiftService.GetShiftsByDoctor(userId, trackChanges: false);
            var resToReturn = new List<string>();
            foreach(var r in res)
            {
                if (resToReturn.Contains(r.ShiftDate))
                    continue;
                else
                    resToReturn.Add(r.ShiftDate);
            }

            return Ok(resToReturn);

        }

        [HttpPost]
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
        }
    }
}
