using Last.Simple.App.Domain.CoreDomain;

namespace Last.Simple.App.Domain.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<long> AddProduct(Product product);
        Task<Product?> Get(long id);
        Task Update(Product product);
    }
}
