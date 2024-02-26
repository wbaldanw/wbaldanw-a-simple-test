using Last.Simple.App.Core.Exceptions;
using Last.Simple.App.Domain.Contracts.Repositories;
using Last.Simple.App.Domain.Contracts.UseCases.Products;
using Last.Simple.App.Domain.Models.Products;

namespace Last.Simple.App.UseCases.Products
{
    public class UpdateProductUC: IUpdateProductUC
    {
        private readonly IProductRepository productRepository;

        public UpdateProductUC(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task Update(long id, UpdateProductRequest request)
        {
            var product = await productRepository.Get(id);

            if (product is null)
                throw new EntityNotFoundException("Product not found");
            
            product.Update(request.Price, request.Description);
            await productRepository.Update(product);
        }
    }
}
