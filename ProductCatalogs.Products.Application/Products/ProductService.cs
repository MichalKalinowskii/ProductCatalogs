using ProductCatalogs.Common.UnitOfWork;
using ProductCatalogs.Products.Application.DTO;
using ProductCatalogs.Products.Application.Interfaces;
using ProductCatalogs.Products.Domain.Models;

namespace ProductCatalogs.Products.Application.Products
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICatalogApi catalogApi;

        public ProductService(IProductRepository productRepository, ICatalogApi catalogApi)
        {
            this.productRepository = productRepository;
            this.catalogApi = catalogApi;
        }

        public async Task<Result<List<ProductResponse>>> GetProductsByCatalogIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var productResult = await productRepository.GetProductsByCatalogIdAsync(categoryId, cancellationToken);

            if (productResult.IsFailure)
            {
                return Result<List<ProductResponse>>.Failure(productResult.Error);
            }

            var productResponses = productResult.Value.Select(p => new ProductResponse
            {
                ProductId = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();

            return Result<List<ProductResponse>>.Success(productResponses);
        }


        public async Task<Result<ProductDetailResponse>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var productResult = await productRepository.GetProductByIdAsync(productId, cancellationToken);

            if (productResult.IsFailure)
            {
                return Result<ProductDetailResponse>.Failure(productResult.Error);
            }

            var productResponse = new ProductDetailResponse
            {
                ProductId = productResult.Value.Id,
                CatalogId = productResult.Value.CatalogId,
                Name = productResult.Value.Name,
                Price = productResult.Value.Price,
                Description = productResult.Value.Description,
                Code = productResult.Value.Code,
            };

            return Result<ProductDetailResponse>.Success(productResponse);
        }


        private async Task<Result<bool>> ValidateProductRequest(ProductRequest product, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return Result<bool>.Failure(new Error("ValidateProductRequest.InvalidName", "Nie podano nazwy produktu."));
            }

            if (product.Price is null || product.Price < 0)
            {
                return Result<bool>.Failure(new Error("ValidateProductRequest.InvalidPrice", "Cena produktu nie może być mniejsza od 0."));
            }

            if (product.CatalogId == Guid.Empty || !await catalogApi.CatalogExistsAsync(product.CatalogId, cancellationToken))
            {
                return Result<bool>.Failure(new Error("ValidateProductRequest.InvalidCatalogId", "Nie prawidłowy katalog."));
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<Guid>> CreateProductAsync(ProductRequest product, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateProductRequest(product, cancellationToken);

            if (validationResult.IsFailure)
            {
                return Result<Guid>.Failure(validationResult.Error);
            }

            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                CatalogId = product.CatalogId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.Value,
                Code = product.Code,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            var result = await productRepository.AddProductAsync(newProduct, cancellationToken);

            return result;
        }

        public async Task<Result<Guid>> UpdateProductAsync(Guid productId, ProductRequest product, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateProductRequest(product, cancellationToken);

            if (validationResult.IsFailure)
            {
                return Result<Guid>.Failure(validationResult.Error);
            }

            var productToUpdate = new Product
            {
                Id = productId,
                CatalogId = product.CatalogId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.Value,
                Code = product.Code,
                UpdatedDate = DateTime.UtcNow
            };

            var result = await productRepository.UpdateProductAsync(productToUpdate, cancellationToken);

            return result;
        }

        public async Task<Result> DeleteProductAsync(Guid productId, CancellationToken cancellationToken)
        {
            var result = await productRepository.DeleteProductAsync(productId, cancellationToken);
            return result;
        }
    }
}
