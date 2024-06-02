using Entities.Models;

namespace Contracts
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviews(bool trackChanges);
        Task<IEnumerable<Review>> GetReviewsForDoctor(string doctorId, bool trackChanges);

        Task<Review> GetReview(Guid reviewId, bool trackChanges);

        Task<Review> GetReviewByShiftId(Guid shiftId, bool trackChanges); 

        void CreateReview(Review review);

        void DeleteReview(Review review);
    }
}
