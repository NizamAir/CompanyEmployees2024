﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ShiftDTOs;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/assistantshifts")]
    [ApiController]
    public class AssistantShiftsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public AssistantShiftsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Assistant")]
        public async Task<IActionResult> GetAllShifts()
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            var res = await _service.ShiftService.GetShiftsByAssistant(userId, trackChanges: false);
            var resToReturn = new List<ShiftAllForAssistantDto>();
            foreach (var r in res)
            {
                var item = new ShiftAllForAssistantDto { DoctorName = r.DoctorName, Date = r.ShiftDate };
                if (resToReturn.Contains(item))
                    continue;
                else
                    resToReturn.Add(item);
            }
            return Ok(resToReturn);



        }

        [HttpGet("doctors")]
        [Authorize(Roles = "Assistant")]
        public async Task<IActionResult> GetDoctors()
        {
            var res = await _service.ShiftService.GetDoctorsForAssistant();

            return Ok(res);

        }

        [HttpGet("dates")]
        [Authorize(Roles = "Assistant")]
        public async Task<IActionResult> GetDays()
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;
            var res = await _service.ShiftService.GetShiftsByAssistant(userId, trackChanges: false);
            var resToReturn = new List<string>();
            foreach (var r in res)
            {
                if (resToReturn.Contains(r.ShiftDate) || DateOnly.FromDateTime(DateTime.Parse(r.ShiftDate)) < DateOnly.FromDateTime(DateTime.Now))
                    continue;
                else
                    resToReturn.Add(r.ShiftDate);
            }

            return Ok(resToReturn);
        }

        [HttpGet("doctordates/{doctorId}")]
        [Authorize(Roles = "Assistant")]
        public async Task<IActionResult> GetDoctorDays(string doctorId)
        {
            var res = await _service.ShiftService.GetShiftsByDoctor(doctorId, trackChanges: false);
            var resToReturn = new List<string>();
            foreach (var r in res)
            {
                if (r.AssistantName != null || DateOnly.Parse(r.ShiftDate) < DateOnly.FromDateTime(DateTime.UtcNow))
                    continue;
                if (resToReturn.Contains(r.ShiftDate))
                    continue;
                else
                    resToReturn.Add(r.ShiftDate);
            }
            return Ok(resToReturn);
        }


        [HttpPost]
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


        [HttpPost("delete")]
        [Authorize(Roles = "Assistant")]
        public async Task<IActionResult> DeleteShiftAssistant([FromBody] ShiftForAssistantDeleteDto shiftForAssistant)
        {
            var userId = HttpContext?.User.FindFirst("Id")?.Value;

            await _service.ShiftService.DeleteShiftAssistant(userId, shiftForAssistant.Date, trackChanges: true);
            
            return NoContent();
        }
    }
}
