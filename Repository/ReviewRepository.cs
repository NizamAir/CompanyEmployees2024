using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Review>> GetAllReviews(bool trackChanges) =>
            await FindAll(trackChanges).OrderByDescending(r => r.CreationDate).ToListAsync();

        public async Task<IEnumerable<Review>> GetReviewsForDoctor(string doctorId, bool trackChanges) =>
            await FindByCondition(r => r.UserId.Equals(doctorId), trackChanges).ToListAsync();
        public async Task<Review> GetReview(Guid reviewId, bool trackChanges) =>
            await FindByCondition(r => r.Id.Equals(reviewId), trackChanges).SingleOrDefaultAsync();

        public async Task<Review> GetReviewByShiftId(Guid shiftId, bool trackChanges) =>
            await FindByCondition(r => r.ShiftId.Equals(shiftId), trackChanges).SingleOrDefaultAsync();
        public void CreateReview(Review review) => Create(review);

        public void DeleteReview(Review review) => Delete(review);


    }
}
