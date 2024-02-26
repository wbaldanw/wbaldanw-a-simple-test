using Last.Simple.App.Domain.Models.Products;

namespace Last.Simple.App.Domain.Contracts.UseCases.Products
{
    public interface ICreateProductUC
    {
        Task<long> Create(CreateProductRequest request);
    }
}
