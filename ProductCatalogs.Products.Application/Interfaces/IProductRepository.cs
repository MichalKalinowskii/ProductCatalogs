using ProductCatalogs.Common.UnitOfWork;
using ProductCatalogs.Products.Domain.Models;

namespace ProductCatalogs.Products.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Result<List<Product>>> GetProductsByCatalogIdAsync(Guid catalogId, CancellationToken cancellationToken);
        Task<Result<Product>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken);
        Task<Result<Guid>> AddProductAsync(Product product, CancellationToken cancellationToken);
        Task<Result<Guid>> UpdateProductAsync(Product product, CancellationToken cancellationToken);
        Task<Result> DeleteProductAsync(Guid productId, CancellationToken cancellationToken);
    }
}
