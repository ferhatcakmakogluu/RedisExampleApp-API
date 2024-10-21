using RedisExampleApp.Api.Model;
using RedisExampleApp.Api.Repository;
using RedisExampleApp.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.Api.Repositories
{
    public class ProductRepositoryWithCach : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _productsRepository;
        private readonly IDatabase _cacheRepository;
        private readonly RedisService _redisService;

        public ProductRepositoryWithCach(IProductRepository productsRepository, RedisService redisService)
        {
            _productsRepository = productsRepository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(0);
        }

        public async Task<Products> CreateAsync(Products product)
        {
            var newProducts = await _productsRepository.CreateAsync(product);
            
            //eger cache de productKey varsa direkt yeni datayı ekle
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, newProducts.Id, JsonSerializer.Serialize(newProducts));
            }


            return newProducts;
        }

        public async Task<List<Products>> GetAsync()
        {
            //data cachde yoksa
            if (!await _cacheRepository.KeyExistsAsync(productKey))
            {
                return await LoadToCacheFromDbAsync();
            }

            var products = new List<Products>();
            var cacheProduct = await _cacheRepository.HashGetAllAsync(productKey);

            foreach(var item in cacheProduct.ToList())
            {
                var product = JsonSerializer.Deserialize<Products>(item.Value);
                products.Add(product);
            }

            return products;
        }

        public async Task<Products> GetByIdAsync(int id)
        {
            if (_cacheRepository.KeyExists(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey,id);
                return product.HasValue ? JsonSerializer.Deserialize<Products>(product) : null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }

        private async Task<List<Products>> LoadToCacheFromDbAsync()
        {
            var products = await _productsRepository.GetAsync();
            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey,p.Id,JsonSerializer.Serialize(p));
            });
            return products;
        }
    }
}
