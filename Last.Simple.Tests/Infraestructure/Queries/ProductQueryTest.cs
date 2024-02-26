using FluentAssertions;
using Last.Simple.App.Domain.Contracts.Services;
using Last.Simple.App.Domain.Models.Products;
using Last.Simple.App.Infra.Queries;
using Last.Simple.App.Infra.Repositories;
using Last.Simple.App.UseCases.Products;
using Last.Simple.Tests.Utils;
using Moq;

namespace Last.Simple.Tests.Infraestructure.Queries
{
    public class ProductQueryTest
    {
        [Fact]
        public async Task Given_IWantGetAllProducts_Then_GetAllProducts()
        {
            //arrange
            var connectionBuilder = new SqlConnectionBuilderForTests();
            var productRepository = new ProductRepository(connectionBuilder);
            var productQuery = new ProductQuery(connectionBuilder, productRepository);

            //act
            var products = await productQuery.ListAll();

            //assert
            products.Should().NotBeNull();
            products.Count().Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task Given_IWantGetProductById_Then_GetProductById()
        {
            //arrange
            var connectionBuilder = new SqlConnectionBuilderForTests();
            var productRepository = new ProductRepository(connectionBuilder);
            var productQuery = new ProductQuery(connectionBuilder, productRepository);

            //arrange
            var request = new CreateProductRequest()
            {
                Name = "product",
                Description = "a product description",
                Price = 10
            };
            
            var loggedUserServiceMock = new Mock<ILoggedUserService>();
            loggedUserServiceMock.Setup(x => x.GetUserId()).Returns(1);

            var uc = new CreateProductUC(productRepository, loggedUserServiceMock.Object);

            var id = await uc.Create(request);

            //act
            var product = await productQuery.Get(id);

            //assert
            product.Should().NotBeNull();
            product.Id.Should().Be(id);
        }
    }
}
