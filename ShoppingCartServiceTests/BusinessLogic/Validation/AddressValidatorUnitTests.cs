using System;
using Xunit;
using ShoppingCartService.BusinessLogic.Validation;

namespace ShoppingCartServiceTests.BusinessLogic.Validation
{
    public class AddressValidatorUnitTests
    {
        [Fact]
        public void IsValid_doesNotHaveCountry_returnFalse()
        {
            var address = TestHelper.CreateAddress(null, "city", "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_doesNotHaveCity_returnFalse()
        {
            var address = TestHelper.CreateAddress("country", null, "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_doesNotHaveStreet_returnFalse()
        {
            var address = TestHelper.CreateAddress("country", "city", null);
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_validValues_returnTrue()
        {
            var address = TestHelper.CreateAddress("country", "city", "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.True(isValid);
        }
    }
}
