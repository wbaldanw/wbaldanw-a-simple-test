using FluentAssertions;
using Last.Simple.App.Domain.CoreDomain;
using Last.Simple.App.Domain.Models.Users;
using Last.Simple.App.Infra.Repositories;
using Last.Simple.App.UseCases.Users;
using Last.Simple.Tests.Utils;

namespace Last.Simple.Tests.UseCases
{
    public class CreateUserUCTest
    {
        [Fact]
        public async Task Given_IWantCreateAnUser_Then_CreateTheUser()
        {
            //arrange
            var userName = "user";
            var password = "password";
            var user = new User(userName, password);

            var connectionBuilder = new SqlConnectionBuilderForTests();
            var repository = new UserRepository(connectionBuilder);
            var uc = new CreateUserUC(repository);

            //act
            var id = await uc.CreateUser(new CreateUserRequest()
            {
                Password = password,
                UserName = userName
            });

            //assert
            var userFromDB = await repository.GetByUserName(userName);

            userFromDB.Should().NotBeNull();
            userFromDB.UserName.Should().Be(userName);
            userFromDB.Password.Should().Be(password);
        }        
    }
}
