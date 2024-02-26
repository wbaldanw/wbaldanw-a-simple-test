using FluentAssertions;
using Last.Simple.App.Domain.CoreDomain;

namespace Last.Simple.Tests.CoreDomain
{
    public class SignInTest
    {
        [Fact]
        public void Given_IWantToSignIn_Then_SignIn()
        {
            //arrange
            var userName = "user";
            var password = "password";
            var user = new User(userName, password);

            var signIn = new SignIn(user);

            //act
            var isAuthenticated = signIn.IsAuthenticated(userName, password);

            //assert
            isAuthenticated.Should().BeTrue();
        }

        [Fact]
        public void Given_IWantToSignIn_And_IProvideAnInvalidPassword_Then_DoNotSignIn()
        {
            //arrange
            var userName = "user";
            var password = "password";
            var user = new User(userName, password);

            var signIn = new SignIn(user);

            //act
            var isAuthenticated = signIn.IsAuthenticated(userName, "invalidPassword");

            //assert
            isAuthenticated.Should().BeFalse();
        }

        [Fact]
        public void Given_IWantToSignIn_And_IProvideAnInvalidUserName_Then_DoNotSignIn()
        {
            //arrange
            var userName = "user";
            var password = "password";
            var user = new User(userName, password);

            var signIn = new SignIn(user);

            //act
            var isAuthenticated = signIn.IsAuthenticated("invalidUserName", password);

            //assert
            isAuthenticated.Should().BeFalse();
        }

        [Fact]
        public void Given_IWantToSignIn_And_UserIsNull_Then_DoNotSignIn()
        {
            //arrange
            User? user = null;

            var signIn = new SignIn(user);

            //act
            var isAuthenticated = signIn.IsAuthenticated("invalidUserName", "invalidPassword");

            //assert
            isAuthenticated.Should().BeFalse();
        }
    }
}
