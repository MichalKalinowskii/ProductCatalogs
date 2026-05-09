using Microsoft.AspNetCore.Mvc;
using ProductCatalogs.Catalogs.Application.Catalogs;
using ProductCatalogs.Catalogs.Application.DTO;
using ProductCatalogs.Catalogs.Domain.Models;
using ProductCatalogs.Products.Application.Products;

namespace ProductCatalogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogService catalogService;

        public CatalogController(CatalogService catalogService)
        {
            this.catalogService = catalogService;
        }

        [HttpGet("catalogs")]
        public async Task<IActionResult> GetCatalogs(CancellationToken cancellationToken)
        {
            var result = await catalogService.GetCatalogsAsync(cancellationToken);
            return result.IsSuccess
                ? (Ok(result.Value))
                : BadRequest(result.Error);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCatalog([FromBody] CatalogRequest catalogRequest, CancellationToken cancellationToken)
        {
            var result = await catalogService.CreateCatalogAsync(catalogRequest, cancellationToken);
            return result.IsSuccess
                ? (Ok(result.Value))
                : BadRequest(result.Error);
        }
        
        [HttpPut("update/{catalogId}")]
        public async Task<IActionResult> UpdateCatalog(Guid catalogId, [FromBody] CatalogRequest catalogRequest, CancellationToken cancellationToken)
        {
            var result = await catalogService.UpdateCatalogAsync(catalogId, catalogRequest, cancellationToken);
            return result.IsSuccess
                ? (Ok(result.Value))
                : BadRequest(result.Error);
        }

        [HttpDelete("delete/{catalogId}")]
        public async Task<IActionResult> DeleteCatalog(Guid catalogId, CancellationToken cancellationToken)
        {
            var result = await catalogService.DeleteCatalogAsync(catalogId, cancellationToken);

            return result.IsSuccess
                ? (Ok(result))
                : BadRequest(result.Error);
        }
    }
}
