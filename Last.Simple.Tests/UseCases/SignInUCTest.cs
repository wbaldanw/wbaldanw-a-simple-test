using FluentAssertions;
using Last.Simple.App.Domain.Models.Users;
using Last.Simple.App.Infra.Repositories;
using Last.Simple.App.UseCases.Users;
using Last.Simple.Tests.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Last.Simple.Tests.UseCases
{
    public class SignInUCTest
    {
        [Fact]
        public async Task Given_IWantSignIn_Then_SignIn()
        {
            //arrange            
            var values = new Dictionary<string, string>
            {
                { "Secret", "SecretSecretSecretSecretSecretSecretSecretSecretSecretSecretSecretSecret" },
                { "ValidIssuer", "ValidAudience" },
                { "ValidAudience", "ValidAudience" }
            };
            var configurationMock = ConfigurationMockBuilder.Build("JWT", values);

            var userName = "test";
            var password = "test";            

            var connectionBuilder = new SqlConnectionBuilderForTests();
            var repository = new UserRepository(connectionBuilder);

            var uc = new SignInUC(repository, configurationMock);

            //act
            var token = await uc.SignIn(new SignInRequest()
            {
                Password = password,
                UserName = userName
            });

            //assert
            token.Should().NotBeNullOrEmpty();
        }
    }
}
