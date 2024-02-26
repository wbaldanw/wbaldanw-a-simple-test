using FluentAssertions;
using Last.Simple.App.Domain.Contracts.Services;
using Last.Simple.App.Domain.Models.Products;
using Last.Simple.App.Infra.Repositories;
using Last.Simple.App.UseCases.Products;
using Last.Simple.Tests.Utils;
using Moq;

namespace Last.Simple.Tests.UseCases
{
    public class UpdateProductUCTest
    {
        [Fact]
        public async Task Given_IWantUpdateAProduct_Then_UpdateTheProduct()
        {
            //arrange
            var request = new UpdateProductRequest()
            {
                Description = "updated a product description",
                Price = 15
            };

            var connectionBuilder = new SqlConnectionBuilderForTests();
            var productRepository = new ProductRepository(connectionBuilder);
            var uc = new UpdateProductUC(productRepository);

            var id = await CreatProduct(connectionBuilder);

            //act
            await uc.Update(id, request);

            //assert
            var product = await productRepository.Get(id);

            product.Price.Should().Be(request.Price);
            product.Description.Should().Be(request.Description);
        }

        private async Task<long> CreatProduct(SqlConnectionBuilderForTests connectionBuilder)
        {
            var request = new CreateProductRequest()
            {
                Name = "product",
                Description = "a product description",
                Price = 10
            };
            
            var productRepository = new ProductRepository(connectionBuilder);
            var loggedUserServiceMock = new Mock<ILoggedUserService>();
            loggedUserServiceMock.Setup(x => x.GetUserId()).Returns(1);

            var uc = new CreateProductUC(productRepository, loggedUserServiceMock.Object);
            
            return await uc.Create(request);
        }
    }
}
