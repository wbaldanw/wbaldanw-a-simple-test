using FluentAssertions;
using Last.Simple.App.Domain.CoreDomain;
using Last.Simple.App.Domain.DataObjects;

namespace Last.Simple.Tests.CoreDomain
{
    public class UserTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("             ")]
        [InlineData(null)]
        public void Given_IDontProvideAUserName_And_IWantCreateAnUser_When_CreateUser_Then_ThrowAnException(string userName)
        {
            //arrange
            Action act = () => new User(userName, "password");

            //assert
            act.Should().Throw<ArgumentException>().WithMessage("The user name is required");
        }


        [Theory]
        [InlineData("")]
        [InlineData("             ")]
        [InlineData(null)]
        public void Given_IDontProvideAPassword_And_IWantCreateAnUser_When_CreateUser_Then_ThrowAnException(string password)
        {
            //arrange
            Action act = () => new User("user", password);

            //assert
            act.Should().Throw<ArgumentException>().WithMessage("The password is required");
        }

        [Fact]
        public void Given_IProvideAUserNameAndPassword_And_IWantCreateAnUser_When_CreateUser_Then_CreateTheUser()
        {
            //arrange
            var userName = "user";
            var password = "password";

            //act
            var user = new User(userName, password);

            //assert
            user.UserName.Should().Be(userName);
            user.Password.Should().Be(password);
        }

        [Fact]
        public void Given_IProvideAUserDO_And_IWantCreateAnUser_When_CreateUser_Then_CreateTheUser()
        {
            //arrange
            var userDO = new UserDO
            {
                Id = 1,
                UserName = "user",
                Password = "password"
            };

            //act
            var user = User.FromDataObject(userDO);

            //assert
            user.Id.Should().Be(userDO.Id);
            user.UserName.Should().Be(userDO.UserName);
            user.Password.Should().Be(userDO.Password);
        }
    }
}
