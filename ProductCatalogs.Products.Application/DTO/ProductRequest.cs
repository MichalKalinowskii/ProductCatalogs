namespace ProductCatalogs.Products.Application.DTO
{
    public class ProductRequest
    {
        public Guid CatalogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Code { get; set; }
    }
}
