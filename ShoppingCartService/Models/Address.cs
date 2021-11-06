using System;

namespace ShoppingCartService.Models
{
    public record Address
    {
        public String Country { get; init; }
        public String City { get; init; }
        public String Street { get; init; }
    }
}