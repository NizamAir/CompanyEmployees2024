using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProducts(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(c => c.Name).ToListAsync();

        public async Task<Product> GetProduct(Guid productId, bool trackChanges) =>
            await FindByCondition(p => p.Id.Equals(productId), trackChanges).SingleOrDefaultAsync();

        public void CreateProduct(Product product) => Create(product);

        public void DeleteProduct(Product product) => Delete(product);
    }
}
