using Microsoft.EntityFrameworkCore;
using ProductCatalogs.Catalogs.Application.Interfaces;
using ProductCatalogs.Catalogs.Domain.Models;
using ProductCatalogs.Common.UnitOfWork;
using ProductCatalogs.Products.Application.Interfaces;
using ProductCatalogs.Products.Domain.Models;

namespace ProductCatalogs.Products.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository, IProductApi
    {
        private readonly ProductDbContext productDbContext;

        public ProductRepository(ProductDbContext productDbContext)
        {
            this.productDbContext = productDbContext;
        }

        public async Task<Result<Guid>> AddProductAsync(Product product, CancellationToken cancellationToken)
        {
            await productDbContext.Products.AddAsync(product, cancellationToken);
            await productDbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(product.Id);
        }

        public async Task<Result> DeleteProductAsync(Guid productId, CancellationToken cancellationToken)
        {
            var productExist = await productDbContext.Products.AnyAsync(x => x.Id == productId, cancellationToken);

            if (!productExist)
            {
                return Result.Failure(new Error("DeleteProductAsync.ProductNotFound", $"Nie znaleziono produktu z id: {productId}"));
            }

            var product = new Product { Id = productId };
            productDbContext.Products.Remove(product);
            await productDbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success();
        }

        public async Task<Result<Product>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await productDbContext.Products.FirstOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if (product == null)
            {
                return Result<Product>.Failure(new Error("GetProductByIdAsync.ProductNotFound", $"Nie znaleziono produktu z id: {productId}"));
            }

            return Result<Product>.Success(product);
        }

        public async Task<Result<List<Product>>> GetProductsByCatalogIdAsync(Guid catalogId, CancellationToken cancellationToken)
        {
            var products = await productDbContext.Products.Where(x => x.CatalogId == catalogId)
                .Select(x => new Product
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price
                })
                .ToListAsync(cancellationToken);

            return Result<List<Product>>.Success(products);
        }

        public async Task<bool> IsCatalogEmptyAsync(Guid catalogId, CancellationToken cancellationToken)
        {
            return !await productDbContext.Products.AnyAsync(x => x.CatalogId == catalogId, cancellationToken);
        }

        public async Task<Result<Guid>> UpdateProductAsync(Domain.Models.Product product, CancellationToken cancellationToken)
        {
            var productExist = await productDbContext.Products.AnyAsync(x => x.Id == product.Id, cancellationToken);

            if (!productExist)
            {
                return Result<Guid>.Failure(new Error("UpdateProductAsync.ProductNotFound", $"Nie znaleziono produktu z id: {product.Id}"));
            }

            productDbContext.Products.Update(product);
            await productDbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(product.Id);
        }
    }
}
