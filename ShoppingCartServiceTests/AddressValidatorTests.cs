using System;
using Xunit;
using ShoppingCartService.Models;
using ShoppingCartService.BusinessLogic.Validation;

namespace ShoppingCartServiceTests
{
    public class AddressValidatorTests
    {
        [Fact]
        public void Validation_with_valid_address()
        {
            var address = createAddress("country", "city", "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.True(isValid);
        }

        [Fact]
        public void Validation_with_null_address_is_invalid()
        {
            var address = (Address) null;
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }


        [Fact]
        public void Validation_with_empty_country_is_invalid()
        {
            var address = createAddress(string.Empty, "city", "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void Validation_with_empty_city_is_invalid()
        {
            var address = createAddress("country", string.Empty, "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void Validation_with_empty_street_is_invalid()
        {
            var address = createAddress("country", "city", string.Empty);
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void Validation_with_null_country_is_invalid()
        {
            var address = createAddress(null, "city", "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void Validation_with_null_city_is_invalid()
        {
            var address = createAddress("country", null, "street");
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void Validation_with_null_street_is_invalid()
        {
            var address = createAddress("country", "city", null);
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        private Address createAddress(string country, string city, string street)
        {
            return new Address { Country = country, City = city, Street = street };
        }
    }
}
