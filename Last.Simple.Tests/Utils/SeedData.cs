using Last.Simple.App.Domain.Contracts.Services;
using Last.Simple.App.Domain.CoreDomain;
using Last.Simple.App.Domain.Models.Products;
using Last.Simple.App.Domain.Models.Users;
using Last.Simple.App.Infra.Repositories;
using Last.Simple.App.UseCases.Products;
using Last.Simple.App.UseCases.Users;
using Moq;

namespace Last.Simple.Tests.Utils
{
    internal class SeedData
    {   
        private readonly SeedDataConnectionBuilder connectionBuilder;

        public SeedData(string connectionString)
        {   
            connectionBuilder = new SeedDataConnectionBuilder(connectionString);
        }

        public async Task Seed() 
        {
            await CreateUsers();
            await CreateProducts();
        }

        private async Task CreateProducts()
        {
            var repository = new ProductRepository(connectionBuilder);
            var loggedUserServiceMock = new Mock<ILoggedUserService>();
            loggedUserServiceMock.Setup(x => x.GetUserId()).Returns(1);

            var p1 = new CreateProductRequest()
            {
                Name = "product",
                Description = "a product description",
                Price = 10
            };

            var p2 = new CreateProductRequest()
            {
                Name = "product2",
                Description = "a product description",
                Price = 20
            };

            var uc = new CreateProductUC(repository, loggedUserServiceMock.Object);

            await uc.Create(p1);
            await uc.Create(p2);
        }

        private async Task CreateUsers()
        {
            var repository = new UserRepository(connectionBuilder);
            var uc = new CreateUserUC(repository);
            await uc.CreateUser(new CreateUserRequest()
            {
                Password = "test",
                UserName = "test"
            });
        }
    }
}
