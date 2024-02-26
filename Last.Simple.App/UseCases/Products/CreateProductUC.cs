using Last.Simple.App.Domain.Contracts.Repositories;
using Last.Simple.App.Domain.Contracts.Services;
using Last.Simple.App.Domain.Contracts.UseCases.Products;
using Last.Simple.App.Domain.CoreDomain;
using Last.Simple.App.Domain.Models.Products;

namespace Last.Simple.App.UseCases.Products
{
    public class CreateProductUC : ICreateProductUC
    {
        private readonly IProductRepository productRepository;
        private readonly ILoggedUserService loggedUserService;

        public CreateProductUC(IProductRepository productRepository, ILoggedUserService loggedUserService)
        {
            this.productRepository = productRepository;
            this.loggedUserService = loggedUserService;
        }

        public async Task<long> Create(CreateProductRequest request)
        {
            var product = new Product(request.Name)
            {
                Description = request.Description,
                Price = request.Price
            };

            product.CreatedBy(loggedUserService.GetUserId() ?? 0);

            return await productRepository.AddProduct(product);
        }
    }
}
