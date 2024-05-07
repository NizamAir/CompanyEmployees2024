namespace Shared.DataTransferObjects.ProductDTOs
{
    public record ProductDto(Guid Id, string Name, string Description, decimal Price, string ImagePath);
}
