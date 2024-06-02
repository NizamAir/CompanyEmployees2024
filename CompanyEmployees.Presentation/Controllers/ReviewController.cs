using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.ReviewDTOs;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IServiceManager _service;
        public ReviewController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles="User")]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _service.ReviewService.GetAllReviews(trackChanges: false);
            return Ok(reviews);
        }

        [HttpGet("doctors/doctorId:guid")]
        public async Task<IActionResult> GetReviewsForDoctor(Guid doctorId)
        {
            var reviews = await _service.ReviewService.GetReviewsForDoctor(doctorId, trackChanges: false);
            return Ok(reviews);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetReview(Guid id)
        {
            var review = await _service.ReviewService.GetReview(id, trackChanges: false);
            return Ok(review);
        }

        [HttpGet("shift/{shiftId:guid}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetReviewByShiftId(Guid shiftId)
        {
            var review = await _service.ReviewService.GetReviewByShiftId(shiftId, trackChanges: false);
            return Ok(review);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateReview([FromBody] ReviewForCreationDto review)
        {
            var userWhoRatedId = HttpContext?.User.FindFirst("Id")?.Value;
            review.UserWhoRatedId = userWhoRatedId;
            if (review is null)
                return BadRequest("CompanyForCreationDto object is null");

            var createdReview = await _service.ReviewService.CreateReview(review);

            return Ok(createdReview);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            await _service.ReviewService.DeleteReview(id, trackChanges: false);
            return NoContent();
        }
    }
}
