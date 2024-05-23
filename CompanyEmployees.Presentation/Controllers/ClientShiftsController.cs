using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ShiftDTOs;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/clientshifts")]
    [ApiController]
    public class ClientShiftsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public ClientShiftsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("doctors")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _service.ShiftService.GetDoctorsForAssistant();
            return Ok(doctors);
        }

        [HttpGet("products")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _service.ProductService.GetAllProducts(trackChanges:false);
            return Ok(products);
        }

        [HttpGet("doctordates/{doctorId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetDoctorDays(string doctorId)
        {
            var res = await _service.ShiftService.GetShiftsByDoctor(doctorId, trackChanges: false);
            var resToReturn = new List<string>();
            foreach (var r in res)
            {
                if (resToReturn.Contains(r.ShiftDate) || DateOnly.Parse(r.ShiftDate) < DateOnly.FromDateTime(DateTime.UtcNow))
                    continue;
                else
                    resToReturn.Add(r.ShiftDate);
            }
            return Ok(resToReturn);
        }

        [HttpPost("times")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetTimesForDay([FromBody] ShiftTimeForDateDto shiftTimeForDate)
        {
            var times = await _service.ShiftService.GetTimesForDate(shiftTimeForDate, trackChanges: false);

            return Ok(times);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateShiftClient([FromBody] ShiftForClientCreationDto shiftForClient)
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            if (shiftForClient.Date.Equals(null))
                return BadRequest("CompanyForCreationDto object is null");

            await _service.ShiftService.UpdateShiftClient(userId, shiftForClient.DoctorId, shiftForClient.ProductId, shiftForClient.Date, trackChanges: true);

            return Ok();
        }

    }
}
