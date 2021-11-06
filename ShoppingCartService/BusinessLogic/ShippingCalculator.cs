using System.Collections.Generic;
using System.Linq;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.BusinessLogic
{
    public interface IShippingCalculator
    {
        public double CalculateShippingCost(Cart cart);
    }

    public class ShippingCalculator : IShippingCalculator
    {
        public const double SameCityRate = 1.0;
        public const double SameCountryRate = 2.0;
        public const double InternationalShippingRate = 15.0;

        private readonly Dictionary<ShippingMethod, double> _shippingIncreaseBasedOnShippingMethod =
            new()
            {
                { ShippingMethod.Standard, 1.0 },
                { ShippingMethod.Expedited, 1.2 },
                { ShippingMethod.Priority, 2.0 },
                { ShippingMethod.Express, 2.5 },
            };

        private readonly Address _warehouseAddress;

        public ShippingCalculator()
        {
            _warehouseAddress = new Address
            {
                Country = "USA",
                City = "Dallas",
                Street = "1234 left lane."
            };
        }

        // If not main office
        public ShippingCalculator(Address warehouseAddress)
        {
            _warehouseAddress = warehouseAddress;
        }

        public double CalculateShippingCost(Cart cart)
        {
            var numberOfItems = (uint)cart.Items.Sum(i => i.Quantity);

            var addressCost = CalculateTravelCost(_warehouseAddress, cart.ShippingAddress, numberOfItems);

            return CalculateShippingMethodCost(addressCost, cart.ShippingMethod, cart.CustomerType);
        }

        private double CalculateShippingMethodCost(double baseCost, ShippingMethod shippingMethod,
            CustomerType customerType)
        {
            if (customerType == CustomerType.Premium)
            {
                if (shippingMethod == ShippingMethod.Priority || shippingMethod == ShippingMethod.Expedited)
                {
                    return baseCost;
                }
            }

            return baseCost * _shippingIncreaseBasedOnShippingMethod[shippingMethod];
        }

        private static double CalculateTravelCost(Address origin, Address destination, uint numberOfItems)
        {
            var sameCountry = origin.Country == destination.Country;
            if (sameCountry)
            {
                if (origin.City == destination.City)
                {
                    return numberOfItems * SameCityRate;
                }

                return numberOfItems * SameCountryRate;
            }

            return numberOfItems * InternationalShippingRate;
        }
    }
}