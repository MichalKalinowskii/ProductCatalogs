namespace ProductCatalogs.Catalogs.Application.Interfaces
{
    public interface IProductApi
    {
        Task<bool> IsCatalogEmptyAsync(Guid catalogId, CancellationToken cancellationToken);
    }
}
