namespace ProductCatalogs.Catalogs.Application.Interfaces
{
    public interface IProductApi
    {
        Task<bool> IsCatalogEmpty(Guid catalogId, CancellationToken cancellationToken);
    }
}
