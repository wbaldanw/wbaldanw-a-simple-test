using Last.Simple.App.Domain.DataObjects;

namespace Last.Simple.App.Domain.CoreDomain
{
    public class Product : Entity
    {
        public Product(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name is required");
            }

            Name = name;
        }

        public string Name { get; private set; }
        
        private decimal price;
        public decimal Price 
        {
            get 
            {
                return price;
            }
            set 
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price cannot be negative");
                }
                price = value;
            }
        }
        public string? Description { get; set; }

        public void Update(decimal price, string? description)
        {
            Description = description;
            Price = price;
        }

        public static Product FromDataObject(ProductDO productDO)
        {
            return new Product(productDO.Name)
            {
                Id = productDO.Id,
                Price = productDO.Price,
                Description = productDO.Description,
                CreatedById = productDO.CreatedById
            };
        }        
    }

    public class Entity : ICreatedBy
    {
        public long Id { get; protected set; }

        public long CreatedById { get; protected set; }

        public void CreatedBy(long userId)
        {
            CreatedById = userId;
        }
    }

    public interface ICreatedBy 
    {
        void CreatedBy(long userId);
    }
}
