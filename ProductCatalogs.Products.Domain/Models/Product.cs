namespace ProductCatalogs.Products.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid CatalogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
