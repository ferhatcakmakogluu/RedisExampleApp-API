using RedisExampleApp.Api.Model;

namespace RedisExampleApp.Api.Services
{
    public interface IProductService
    {
        Task<List<Products>> GetAsync();
        Task<Products> GetByIdAsync(int id);
        Task<Products> CreateAsync(Products product);
    }
}
