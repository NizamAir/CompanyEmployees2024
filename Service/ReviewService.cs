using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects.ReviewDTOs;

namespace Service
{
    public sealed class ReviewService : IReviewService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ReviewService(IRepositoryManager repository, IMapper mapper, UserManager<User> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviews(bool trackChanges)
        {
            var reviews = await _repository.Review.GetAllReviews(trackChanges);

            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return reviewsDto;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForDoctor(Guid doctorId, bool trackChanges)
        {
            var doctor = await _repository.Doctor.GetDoctor(doctorId, trackChanges);
            var reviews = await _repository.Review.GetReviewsForDoctor(doctor.DoctorId, trackChanges);

            foreach(var review in reviews)
            {
                if (review.UserWhoRatedId != null)
                    review.UserWhoRated = await _userManager.FindByIdAsync(review.UserWhoRatedId);
            }

            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return reviewsDto;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForDoctorStat(string doctorId, bool trackChanges)
        {
           
            var reviews = await _repository.Review.GetReviewsForDoctor(doctorId, trackChanges);

            foreach (var review in reviews)
            {
                if (review.UserWhoRatedId != null)
                    review.UserWhoRated = await _userManager.FindByIdAsync(review.UserWhoRatedId);
            }

            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return reviewsDto;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForDoctorPage(string doctorId, bool trackChanges)
        {
            var reviews = await _repository.Review.GetReviewsForDoctor(doctorId, trackChanges);

            foreach (var review in reviews)
            {
                if (review.UserWhoRatedId != null)
                    review.UserWhoRated = await _userManager.FindByIdAsync(review.UserWhoRatedId);
            }

            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return reviewsDto;
        }

        public async Task<ReviewDto> GetReview(Guid reviewId, bool trackChanges)
        {
            var review = await _repository.Review.GetReview(reviewId, trackChanges);

            if (review is null)
                throw new CompanyNotFoundException(reviewId);

            var reviewDto = _mapper.Map<ReviewDto>(review);

            return reviewDto;
        }

        public async Task<ReviewDto> GetReviewByShiftId(Guid shiftId, bool trackChanges)
        {
            var review = await _repository.Review.GetReviewByShiftId(shiftId, trackChanges);
            if (review is null)
                throw new CompanyNotFoundException(shiftId);
            var reviewDto = _mapper.Map<ReviewDto>(review);
            return reviewDto;
        }

        public async Task<ReviewDto> CreateReview(ReviewForCreationDto review)
        {
            review.CreationDate = DateTime.Now;
            var shift = await _repository.Shift.GetShift(review.ShiftId, trackChanges: false);
            review.UserId = shift.DoctorId;
            var reviewEntity = _mapper.Map<Review>(review);

            _repository.Review.CreateReview(reviewEntity);
            await _repository.SaveAsync();

            var reviewToReturn = _mapper.Map<ReviewDto>(reviewEntity);
            return reviewToReturn;
        }

        public async Task DeleteReview(Guid reviewId, bool trackChanges)
        {
            var review = await _repository.Review.GetReview(reviewId, trackChanges);
            if (review is null)
                throw new CompanyNotFoundException(reviewId);

            _repository.Review.DeleteReview(review);
            await _repository.SaveAsync();
        }




    }
}
