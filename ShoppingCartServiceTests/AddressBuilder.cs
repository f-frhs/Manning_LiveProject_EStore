using System;
using ShoppingCartService.Models;

namespace ShoppingCartServiceTests
{
    internal class AddressBuilder
    {
        private string _country;
        private string _city;
        private string _street;

        private AddressBuilder()
        {
        }

        internal static AddressBuilder OfDefault()
        {
            return new AddressBuilder()
            {
                _country = "the country",
                _city = "the city",
                _street = "the street",
            };
        }

        internal AddressBuilder(Address address)
        {
            _country = address.Country;
            _city = address.City;
            _street = address.Street;
        }

        internal AddressBuilder WithCountry(string country)
        {
            _country = country;
            return this;
        }

        internal AddressBuilder WithCity(string city)
        {
            _city = city;
            return this;
        }

        internal AddressBuilder WithStreet(string street)
        {
            _street = street;
            return this;
        }

        internal Address Build()
        {
            return new Address {Country = _country, City = _city, Street = _street};
        }
    }
}