using Microsoft.AspNetCore.Http;

namespace Shared.DataTransferObjects.ProductDTOs
{
    public record ProductForCreationDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string strPrice { get; init; }
        public string? ImagePath { get; init; }
        public IFormFile ImageFile { get; init; }
    }

}
