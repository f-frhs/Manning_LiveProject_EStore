using System;
using Xunit;
using ShoppingCartService.Models;
using ShoppingCartService.BusinessLogic.Validation;

namespace ShoppingCartServiceTests.BusinessLogic.Validation
{
    public class AddressValidatorTests
    {
        [Fact]
        public void IsValid_doesNotHaveCountry_returnFalse()
        {
            var address = createAddress(null, "city", "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_doesNotHaveCity_returnFalse()
        {
            var address = createAddress("country", null, "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_doesNotHaveStreet_returnFalse()
        {
            var address = createAddress("country", "city", null);
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_validValues_returnTrue()
        {
            var address = createAddress("country", "city", "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.True(isValid);
        }

        private Address createAddress(string country, string city, string street)
        {
            return new Address { Country = country, City = city, Street = street };
        }
    }
}
