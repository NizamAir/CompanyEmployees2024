using Entities.Models;

namespace Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts(bool trackChanges);

        Task<Product> GetProduct(Guid productId, bool trackChanges);

        void CreateProduct(Product product);

        void DeleteProduct(Product product);
    }
}
