namespace Last.Simple.App.Domain.DataObjects
{
    public class ProductDO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public long CreatedById { get; set; }
    }
}
