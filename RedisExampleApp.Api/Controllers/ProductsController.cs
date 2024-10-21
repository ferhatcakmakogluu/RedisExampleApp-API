using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.Api.Model;
using RedisExampleApp.Api.Repository;
using RedisExampleApp.Caching;

namespace RedisExampleApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;

        public ProductsController(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Products products)
        {
            return Created(string.Empty, await _productRepository.CreateAsync(products));
        }
    }
}
