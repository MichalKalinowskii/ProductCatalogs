namespace ProductCatalogs.Products.Application.DTO
{
    public class ProductDetailResponse
    {
        public Guid ProductId { get; set; }
        public Guid CatalogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Code { get; set; }
    }
}
