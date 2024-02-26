using Last.Simple.App.Domain.Views;

namespace Last.Simple.App.Domain.Contracts.Queries
{
    public interface IProductQuery
    {
        Task<ProductView?> Get(long id);
        Task<List<ProductView>> ListAll();
    }
}
