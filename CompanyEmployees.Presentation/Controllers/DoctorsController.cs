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
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _service.DoctorService.GetAllDoctors(trackChanges: false);
            var res = new List<DoctorToReturnDto>();
            foreach (var doctor in doctors)
            {
                var reviews = await _service.ReviewService.GetReviewsForDoctor(doctor.id, trackChanges: false);
                double rating = 0;
                double count = 0;
                foreach (var review in reviews)
                {
                    rating += review.StarsCount;
                    count++;
                }
                double resRating;
                if(count>0)
                    resRating = rating / count;
                else
                    resRating = 0;
                string resRatingStr;
                if (resRating == 0 || resRating == 1 || resRating == 2 || resRating == 3 || resRating == 4 || resRating == 5)
                    resRatingStr = resRating + ",0";
                else
                    resRatingStr = resRating.ToString();
                string resRatingStrToReturn = resRatingStr.Split(',')[0]+"."+ resRatingStr.Split(',')[1];

                res.Add(new DoctorToReturnDto { Doctor = doctor, Reviews = reviews, Rating = resRatingStrToReturn });
            }

            return Ok(res);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDoctorsForAdmin()
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            await _service.DoctorService.DeleteDoctor(id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateDoctor(Guid id, [FromForm] DoctorForUpdateDto doctor)
        {
            if (doctor is null)
                return BadRequest("DoctorForUpdateDto object is null");

            await _service.DoctorService.UpdateDoctor(id, doctor, trackChanges: true);
            return NoContent();
        }
    }
}
