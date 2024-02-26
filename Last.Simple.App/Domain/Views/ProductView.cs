using Last.Simple.App.Domain.CoreDomain;

namespace Last.Simple.App.Domain.Views
{
    public class ProductView
    {
        public ProductView() { }
        public ProductView(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Description = product.Description;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
