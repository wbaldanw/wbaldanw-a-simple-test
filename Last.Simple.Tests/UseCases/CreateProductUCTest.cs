using FluentAssertions;
using Last.Simple.App.Domain.Contracts.Services;
using Last.Simple.App.Domain.Models.Products;
using Last.Simple.App.Infra.Repositories;
using Last.Simple.App.UseCases.Products;
using Last.Simple.Tests.Utils;
using Moq;

namespace Last.Simple.Tests.UseCases
{
    public class CreateProductUCTest
    {
        [Fact]
        public async Task Given_IWantCreateAProduct_Then_CreateTheProduct()
        {
            //arrange
            var request = new CreateProductRequest()
            {
                Name = "product",
                Description = "a product description",
                Price = 10
            };

            var connectionBuilder = new SqlConnectionBuilderForTests();
            var productRepository = new ProductRepository(connectionBuilder);
            var loggedUserServiceMock = new Mock<ILoggedUserService>();
            loggedUserServiceMock.Setup(x => x.GetUserId()).Returns(1);

            var uc = new CreateProductUC(productRepository, loggedUserServiceMock.Object);

            //act
            var id = await uc.Create(request);

            //assert
            var product = await productRepository.Get(id);

            product.Name.Should().Be(request.Name);
            product.Price.Should().Be(request.Price);
            product.Description.Should().Be(request.Description);
            product.CreatedById.Should().Be(1);
        }
    }
}
