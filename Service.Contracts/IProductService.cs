using Shared.DataTransferObjects.ProductDTOs;

namespace Service.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts(bool trackChanges);
        Task<ProductDto> GetProduct(Guid productId, bool trackChanges);
        Task<ProductDto> CreateProduct(ProductForCreationDto product);
        Task DeleteProduct(Guid productId, bool trackChanges);
        Task UpdateProduct(Guid productId, ProductForUpdateDto productForUpdate, bool trackChanges);

    }
}
