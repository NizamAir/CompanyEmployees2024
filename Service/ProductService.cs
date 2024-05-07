using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Shared.DataTransferObjects.ProductDTOs;

namespace Service
{
    public sealed class ProductService : IProductService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ProductService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProducts(bool trackChanges)
        {
            var products = await _repository.Product.GetAllProducts(trackChanges);

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return productsDto;
        }
        public async Task<ProductDto> GetProduct(Guid productId, bool trackChanges)
        {
            var product = await _repository.Product.GetProduct(productId, trackChanges);

            if (product is null)
                throw new CompanyNotFoundException(productId);

            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        public async Task<ProductDto> CreateProduct(ProductForCreationDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            productEntity.ImagePath = await SaveImage(product.ImageFile);
            var tmpPrice = product.strPrice;
            var splPrice = tmpPrice.Split(".");
            var normPrice = splPrice[0] + "," + splPrice[1];
            var decPrice = decimal.Parse(normPrice);
            productEntity.Price = decPrice;

            _repository.Product.CreateProduct(productEntity);
            await _repository.SaveAsync();

            var productToReturn = _mapper.Map<ProductDto>(productEntity);
            return productToReturn;
        }
        public async Task UpdateProduct(Guid productId, ProductForUpdateDto productForUpdate, bool trackChanges)
        {
            var productEntity = await _repository.Product.GetProduct(productId, trackChanges);

            var tmpPrice = productForUpdate.strPrice;

            var splPrice = tmpPrice.Split(".");
            if (splPrice.Length > 1)
            {
                var normPrice = splPrice[0] + "," + splPrice[1];
                var decPrice = decimal.Parse(normPrice);
                productEntity.Price = decPrice;
            }
            else
            {
                var normPrice = splPrice[0];
                var decPrice = decimal.Parse(normPrice);
                productEntity.Price = decPrice;
            }
            if (productForUpdate.ImageFile != null)
            {
                DeleteImage(productEntity.ImagePath);
                productForUpdate.ImagePath = await SaveImage(productForUpdate.ImageFile);
            }
            if (productEntity is null)
                throw new CompanyNotFoundException(productId);

            _mapper.Map(productForUpdate, productEntity);
            await _repository.SaveAsync();
        }

        public async Task DeleteProduct(Guid productId, bool trackChanges)
        {
            var product = await _repository.Product.GetProduct(productId, trackChanges);
            if (product is null)
                throw new CompanyNotFoundException(productId);
            DeleteImage(product.ImagePath);
            _repository.Product.DeleteProduct(product);
            await _repository.SaveAsync();
        }



        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine("C:\\Users\\Мой компьютер\\source\\repos\\CompanyEmployees2024\\CompanyEmployees2024", "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine("C:\\Users\\Мой компьютер\\source\\repos\\CompanyEmployees2024\\CompanyEmployees2024", "Images", imageName);
            if (File.Exists(imagePath))
                File.Delete(imagePath);
        }
    }
}
