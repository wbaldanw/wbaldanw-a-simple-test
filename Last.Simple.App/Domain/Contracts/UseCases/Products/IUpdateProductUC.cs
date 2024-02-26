using Last.Simple.App.Domain.Models.Products;

namespace Last.Simple.App.Domain.Contracts.UseCases.Products
{
    public interface IUpdateProductUC
    {
        Task Update(long id, UpdateProductRequest request);
    }
}
