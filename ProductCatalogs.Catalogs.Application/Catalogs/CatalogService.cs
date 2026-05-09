using ProductCatalogs.Catalogs.Application.DTO;
using ProductCatalogs.Catalogs.Application.Interfaces;
using ProductCatalogs.Catalogs.Domain.Models;
using ProductCatalogs.Common.UnitOfWork;

namespace ProductCatalogs.Catalogs.Application.Catalogs
{
    public class CatalogService
    {
        private readonly ICatalogRepository catalogRepository;
        private readonly IProductApi productApi;

        public CatalogService(ICatalogRepository catalogRepository, IProductApi productApi)
        {
            this.catalogRepository = catalogRepository;
            this.productApi = productApi;
        }

        public async Task<Result<List<CatalogResponse>>> GetCatalogsAsync(CancellationToken cancellationToken)
        {
            var result = await catalogRepository.GetCatalogsAsync(cancellationToken);

            if (result.IsFailure)
            {
                return Result<List<CatalogResponse>>.Failure(result.Error);
            }

            var catalogResponses = result.Value.Select(c => new CatalogResponse
            {
                CatalogId = c.Id,
                Name = c.Name,
            }).ToList();

            return Result<List<CatalogResponse>>.Success(catalogResponses);
        }


        private Result<bool> IsValidCatalogRequest(CatalogRequest catalogRequest)
        {
            if (string.IsNullOrWhiteSpace(catalogRequest.Name))
            {
                return Result<bool>.Failure(new Error("CreateCatalogAsync.InvalidName", "Nie podano nazwy katalogu."));
            }

            if (catalogRequest.Name.Length > 100)
            {
                return Result<bool>.Failure(new Error("CreateCatalogAsync.NameTooLong", "Nazwa katalogu nie może przekraczać 100 znaków."));
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<Guid>> CreateCatalogAsync(CatalogRequest catalogRequest, CancellationToken cancellationToken)
        {
            var validationResult = IsValidCatalogRequest(catalogRequest);

            if (validationResult.IsFailure) 
            {
                return Result<Guid>.Failure(validationResult.Error);
            }

            var newCatalog = new Catalog
            {
                Id = Guid.NewGuid(),
                Name = catalogRequest.Name,
            };

            return await catalogRepository.CreateCatalogAsync(newCatalog, cancellationToken);
        }

        public async Task<Result<Guid>> UpdateCatalogAsync(Guid catalogId, CatalogRequest catalogRequest, CancellationToken cancellationToken)
        {
            var validationResult = IsValidCatalogRequest(catalogRequest);

            if (validationResult.IsFailure)
            {
                return Result<Guid>.Failure(validationResult.Error);
            }

            if (catalogId == Guid.Empty) 
            {
                return Result<Guid>.Failure(new Error("UpdateCatalogAsync.InvalidCatalogId", "Nie prawidłowy katalog."));
            }

            var catalogToUpdate = new Catalog
            {
                Id = catalogId,
                Name = catalogRequest.Name,
            };

            return await catalogRepository.UpdateCatalogAsync(catalogToUpdate, cancellationToken);
        }

        public async Task<Result> DeleteCatalogAsync(Guid catalogId, CancellationToken cancellationToken)
        {
            var result = await catalogRepository.DeleteCatalogAsync(catalogId, cancellationToken);
            return result;
        }
    }
}
