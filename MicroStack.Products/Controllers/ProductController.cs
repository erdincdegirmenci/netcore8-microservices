using Microsoft.AspNetCore.Mvc;
using MicroStack.Products.Entities;
using MicroStack.Products.Repositories.Interfaces;
using System.Net;

namespace MicroStack.Products.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetProducts());
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(string id)
        {
            var products = await _productRepository.GetProduct(id);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<Product>>> Update([FromBody] Product product)
        {
            return Ok(await _productRepository.UpdateProduct(product));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> Delete(string id)
        {
            return Ok(await _productRepository.DeleteProduct(id));
        }
    }
}
