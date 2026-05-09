using ProductCatalogs.Catalogs.Domain.Models;
using ProductCatalogs.Common.UnitOfWork;

namespace ProductCatalogs.Catalogs.Application.Interfaces
{
    public interface ICatalogRepository
    {
        Task<Result<List<Catalog>>> GetCatalogsAsync(CancellationToken cancellationToken);
        Task<Result<Guid>> CreateCatalogAsync(Catalog catalog, CancellationToken cancellationToken);
        Task<Result<Guid>> UpdateCatalogAsync(Catalog catalog, CancellationToken cancellationToken);
        Task<Result> DeleteCatalogAsync(Guid catalogId, CancellationToken cancellationToken);
    }
}
