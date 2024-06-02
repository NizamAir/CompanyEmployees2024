using Shared.DataTransferObjects.ReviewDTOs;

namespace Service.Contracts
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllReviews(bool trackChanges);
        Task<IEnumerable<ReviewDto>> GetReviewsForDoctor(Guid doctorId, bool trackChanges);
        Task<IEnumerable<ReviewDto>> GetReviewsForDoctorStat(string doctorId, bool trackChanges);
        Task<IEnumerable<ReviewDto>> GetReviewsForDoctorPage(string doctorId, bool trackChanges);
        Task<ReviewDto> GetReview(Guid reviewId, bool trackChanges);
        Task<ReviewDto> GetReviewByShiftId(Guid shiftId, bool trackChanges);
        Task<ReviewDto> CreateReview(ReviewForCreationDto review);
        Task DeleteReview(Guid reviewId, bool trackChanges);
    }
}
