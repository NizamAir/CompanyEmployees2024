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

        [HttpGet("shifts")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAllShifts()
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            var res = await _service.ShiftService.GetShiftsByDoctor(userId, trackChanges: false);
            var resToReturn = new List<ShiftAllForDoctorDto>();
            foreach (var r in res)
            {
                var item = new ShiftAllForDoctorDto { ClientName = r.ClientName, AssistantName = r.AssistantName, Date = r.ShiftDate };
                if (r.ClientName != null)
                    item.ClientName = null;
                else
                {
                    if (resToReturn.Count > 0 && resToReturn.Contains(item))
                        item.ClientName = resToReturn.FirstOrDefault(s => s.AssistantName == item.AssistantName && s.Date == item.Date).ClientName;
                }
                if (r.ClientName == null && resToReturn.Contains(item))
                    continue;
                else if (r.ClientName != null && resToReturn.Contains(item))
                {
                    resToReturn.RemoveAll(s => s.AssistantName == item.AssistantName && s.Date == item.Date);
                    item.ClientName = r.ClientName;
                    resToReturn.Add(item);
                }
                else
                    resToReturn.Add(item);
            }
            for (var i = 0; i < resToReturn.Count; i++)
            {
                var toDeleteItem = resToReturn.FirstOrDefault(s => s.AssistantName == resToReturn[i].AssistantName && s.Date == resToReturn[i].Date && s.ClientName == null);
                if (resToReturn[i].ClientName != null)
                {
                    resToReturn.Remove(toDeleteItem);
                    i++;
                }
            }
            return Ok(resToReturn);

        }

        /*[HttpGet("clients")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetClients()
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            var res = await _service.ShiftService.GetShiftsByDoctor(userId, trackChanges: false);
            var resToReturn = res.Where(r => DateOnly.FromDateTime(DateTime.Parse(r.ShiftDate)) >= DateOnly.FromDateTime(DateTime.UtcNow)).ToList();
                
            return Ok(resToReturn);

        }*/

        [HttpPost("clients")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetClientsPerDay([FromBody] ShiftForClientsInDayDto clientsInDayDto)
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            var res = await _service.ShiftService.GetShiftsByDoctor(userId, trackChanges: false);
            var resToReturn = res.Where(r => DateOnly.FromDateTime(DateTime.Parse(r.ShiftDate)) == DateOnly.FromDateTime(DateTime.Parse(clientsInDayDto.Date))).ToList();

            return Ok(resToReturn);

        }

        [HttpGet("dates")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorDays()
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            var res = await _service.ShiftService.GetShiftsByDoctor(userId, trackChanges: false);
            var resToReturn = new List<string>();
            foreach (var r in res)
            {
                if (resToReturn.Contains(r.ShiftDate))
                    continue;
                else
                    resToReturn.Add(r.ShiftDate);
            }
            return Ok(resToReturn);

        }

        [HttpGet("reviews")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorReviews()
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;

            var res = await _service.ReviewService.GetReviewsForDoctorPage(userId, trackChanges: false);
            
            return Ok(res);

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

        [HttpPost("delete")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DeleteShiftDoctor([FromBody] ShiftForAssistantDeleteDto shiftForDoctor)
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;

            await _service.ShiftService.DeleteShiftDoctor(userId, shiftForDoctor.Date, trackChanges: true);

            return NoContent();
        }
    }
}
