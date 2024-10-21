using RedisExampleApp.Api.Model;

namespace RedisExampleApp.Api.Repository
{
    public interface IProductRepository
    {
        Task<List<Products>> GetAsync();
        Task<Products> GetByIdAsync(int id);
        Task<Products> CreateAsync(Products product);
    }
}
