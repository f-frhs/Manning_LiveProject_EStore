using Xunit;
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;

namespace ShoppingCartServiceTests.BusinessLogic.Validation
{
    public class AddressValidatorUnitTests
    {
        private static readonly Address ValidAddress = AddressBuilder.OfDefault().Build();

        [Fact]
        public void IsValid_doesNotHaveCountry_returnFalse()
        {
            var address = new AddressBuilder(ValidAddress).WithCountry(null).Build();
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_doesNotHaveCity_returnFalse()
        {
            var address = new AddressBuilder(ValidAddress).WithCity(null).Build();
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_doesNotHaveStreet_returnFalse()
        {
            var address = new AddressBuilder(ValidAddress).WithStreet(null).Build();
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_validValues_returnTrue()
        {
            var address = ValidAddress;
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.True(isValid);
        }
    }
}
