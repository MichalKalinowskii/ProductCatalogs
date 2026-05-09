using Microsoft.AspNetCore.Mvc;
using ProductCatalogs.Products.Application.DTO;
using ProductCatalogs.Products.Application.Products;
using ProductCatalogs.Products.Domain.Models;

namespace ProductCatalogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductController(ProductService productLogic)
        {
            this.productService = productLogic;
        }

        [HttpGet("products/{categoryId}")]
        public async Task<IActionResult> GetProducts(Guid categoryId, CancellationToken cancellationToken)
        {
            var result = await productService.GetProductsByCatalogIdAsync(categoryId, cancellationToken);

            return result.IsSuccess
                ? (Ok(result.Value))
                : BadRequest(result.Error);
        }   

        [HttpGet("details/{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId, CancellationToken cancellationToken)
        {
            var result = await productService.GetProductByIdAsync(productId, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest product, CancellationToken cancellationToken)
        {
            var result = await productService.CreateProductAsync(product, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] ProductRequest product, CancellationToken cancellationToken)
        {
            var result = await productService.UpdateProductAsync(productId, product, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId, CancellationToken cancellationToken)
        {
            var result = await productService.DeleteProductAsync(productId, cancellationToken);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result.Error);
        }
    }
}