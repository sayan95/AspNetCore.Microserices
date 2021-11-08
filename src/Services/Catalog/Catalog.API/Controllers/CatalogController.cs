using Catalog.API.Entities;
using Catalog.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    public class CatalogController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        
        // get product list
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)] 
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() {
            var products = await _productRepository.GetProducts();
            return Ok(new{products = products });
        }

        // get product by id
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id){
            var product = await _productRepository.GetProduct(id);
            if(product == null){
                _logger.LogError($"Product with id {id}, not found");
                return NotFound();
            }
             return Ok(new{product = product });
        }

        // get product by categoryname
        [Route("products/{category}", Name = "GetProductsByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category){
            var products = await _productRepository.GetProductByCategory(category);
            return Ok(products);
        }

        // create new product
        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product){
           await _productRepository.CreateProduct(product);
           return CreatedAtRoute("GetProduct", new { id = product.Id}, product); 
        }

        // upadate product
        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product){
            var result = await _productRepository.UpdateProduct(product);
            return result ? Ok() : BadRequest("product update failed");  
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id){
            return Ok(await _productRepository.DeleteProduct(id));   
        }
    }
}
