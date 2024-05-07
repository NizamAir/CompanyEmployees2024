using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ProductDTOs;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ProductsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _service.ProductService.GetAllProducts(trackChanges: false);

            return Ok(products);
        }

        [HttpGet("{id:guid}", Name = "ProductById")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _service.ProductService.GetProduct(id, trackChanges: false);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductForCreationDto product)
        {

            if (product is null)
                return BadRequest("ProductForCreationDto object is null");

            var createdProduct = await _service.ProductService.CreateProduct(product);

            /*return CreatedAtRoute("ProductById", new { id = createdProduct.Id }, createdProduct);*/
            return Ok(createdProduct);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _service.ProductService.DeleteProduct(id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromForm] ProductForUpdateDto product)
        {
            if (product is null)
                return BadRequest("ProductForUpdateDto object is null");

            await _service.ProductService.UpdateProduct(id, product, trackChanges: true);
            return NoContent();
        }


    }
}
