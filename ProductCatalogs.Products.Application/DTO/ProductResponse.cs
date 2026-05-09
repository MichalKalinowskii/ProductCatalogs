namespace ProductCatalogs.Products.Application.DTO
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
