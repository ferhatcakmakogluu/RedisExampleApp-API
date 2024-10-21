using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Api.Model;

namespace RedisExampleApp.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Products> CreateAsync(Products product)
        {
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<List<Products>> GetAsync()
        {
            return await _appDbContext.Products.ToListAsync();
        }

        public async Task<Products> GetByIdAsync(int id)
        {
            return await _appDbContext.Products.FindAsync(id);
        }
    }
}
