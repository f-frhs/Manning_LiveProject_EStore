using System;
using ShoppingCartService.Models;

namespace ShoppingCartServiceTests
{
    internal class AddressBuilder
    {
        private string _country;
        private string _city;
        private string _street;

        internal AddressBuilder()
        {
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