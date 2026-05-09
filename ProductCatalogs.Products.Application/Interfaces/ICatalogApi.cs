namespace ProductCatalogs.Products.Application.Interfaces
{
    public interface ICatalogApi
    {
        Task<bool> CatalogExistsAsync(Guid catalogId, CancellationToken cancellationToken);
    }
}
