using FluentAssertions;
using Last.Simple.App.Domain.CoreDomain;
using Last.Simple.App.Domain.Views;

namespace Last.Simple.Tests.CoreDomain
{
    public class ProductTest
    {
        [Fact]
        public void Given_IWantCreateAProduct_Then_CreateTheProduct()
        {
            //arrange & act
            var productName = "product";
            var description = "a product description";

            var product = new Product(productName)
            {                
                Price = 10,
                Description = description
            };            

            //assert
            product.Name.Should().Be(productName);
            product.Price.Should().Be(10);
            product.Description.Should().Be(description);
        }

        [Fact]
        public void Given_IWantCreateAProductWithInvalidName_Then_ThrowException()
        {
            //arrange
            var productName = "";

            //act
            Action act = () => new Product(productName);

            //assert
            act.Should().Throw<ArgumentException>().WithMessage("Product name is required");
        }

        [Fact]
        public void Given_IWantCreateAProductWithInvalidPrice_Then_ThrowException()
        {
            //arrange
            var productName = "product";
            var product = new Product(productName);

            //act
            Action act = () => product.Price = -1;

            //assert
            act.Should().Throw<ArgumentException>().WithMessage("Price cannot be negative");
        }

        [Fact]
        public void Given_IWantUpdateAProduct_Then_UpdateTheProduct()
        {
            //arrange
            var productName = "product";
            var product = new Product(productName);

            //act
            product.Update(10, "a product description");

            //assert
            product.Price.Should().Be(10);
            product.Description.Should().Be("a product description");
        }

        [Fact]
        public void Given_IWantToUpdateAProductWithInvalidPrice_Then_ThrowException()
        {
            //arrange
            var productName = "product";
            var product = new Product(productName);

            //act
            Action act = () => product.Update(-1, "a product description");

            //assert
            act.Should().Throw<ArgumentException>().WithMessage("Price cannot be negative");
        }

        [Fact]
        public void Given_IWantToGetTheProductView_Then_GetTheProductView()
        {
            //arrange
            var productName = "product";
            var product = new Product(productName)
            {
                Price = 10,
                Description = "a product description",
            };

            //act
            var productView = new ProductView(product);

            //assert
            productView.Name.Should().Be(product.Name);
            productView.Price.Should().Be(product.Price);
            productView.Description.Should().Be(product.Description);
        }
    }
}
