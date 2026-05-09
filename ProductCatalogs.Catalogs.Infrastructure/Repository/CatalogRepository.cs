using Microsoft.EntityFrameworkCore;
using ProductCatalogs.Catalogs.Application.Interfaces;
using ProductCatalogs.Catalogs.Domain.Models;
using ProductCatalogs.Common.UnitOfWork;
using ProductCatalogs.Products.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogs.Catalogs.Infrastructure.Repository
{
    public class CatalogRepository : ICatalogApi, ICatalogRepository
    {
        private readonly CatalogDbContext catalogDbContext;

        public CatalogRepository(CatalogDbContext catalogDbContext)
        {
            this.catalogDbContext = catalogDbContext;
        }

        public async Task<bool> CatalogExistsAsync(Guid catalogId, CancellationToken cancellationToken)
        {
            return await catalogDbContext.Catalogs.AnyAsync(c => c.Id == catalogId, cancellationToken);
        }

        public async Task<Result<Guid>> CreateCatalogAsync(Catalog catalog, CancellationToken cancellationToken)
        {
            await catalogDbContext.Catalogs.AddAsync(catalog, cancellationToken);
            await catalogDbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(catalog.Id);
        }

        public async Task<Result> DeleteCatalogAsync(Guid catalogId, CancellationToken cancellationToken)
        {
            var catalogExist = await catalogDbContext.Catalogs.AnyAsync(x => x.Id == catalogId, cancellationToken);

            if (!catalogExist)
            {
                return Result.Failure(new Error("DeleteCatalogAsync.CatalogNotFound", $"Nie znaleziono katalogu z id: {catalogId}"));
            }

            var catalog = new Catalog { Id = catalogId };
            catalogDbContext.Catalogs.Remove(catalog);
            await catalogDbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success();
        }

        public async Task<Result<List<Catalog>>> GetCatalogsAsync(CancellationToken cancellationToken)
        {
            var catalogs = await catalogDbContext.Catalogs.ToListAsync(cancellationToken);

            return Result<List<Catalog>>.Success(catalogs);
        }

        public async Task<Result<Guid>> UpdateCatalogAsync(Catalog catalog, CancellationToken cancellationToken)
        {
            var catalogExist = await catalogDbContext.Catalogs.AnyAsync(x => x.Id == catalog.Id, cancellationToken);

            if (!catalogExist)
            {
                return Result<Guid>.Failure(new Error("UpdateCatalogAsync.CatalogNotFound", $"Nie znaleziono katalogu z id: {catalog.Id}"));
            }

            catalogDbContext.Catalogs.Update(catalog);
            await catalogDbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(catalog.Id);
        }
    }
}
