using RedisExampleApp.Api.Model;
using RedisExampleApp.Api.Repository;

namespace RedisExampleApp.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Products> CreateAsync(Products product)
        {
            await _productRepository.CreateAsync(product);
            return product;
        }

        public async Task<List<Products>> GetAsync()
        {
            return await _productRepository.GetAsync();
        }

        public async Task<Products> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }
    }
}
