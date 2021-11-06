using System.ComponentModel.DataAnnotations;
using ShoppingCartService.Models;

namespace ShoppingCartService.Controllers.Models
{
    public record CustomerDto
    {
        [Required]
        public string Id { get; init; }

        [Required]
        public Address Address { get; init; }

        [Required]
        public CustomerType CustomerType { get; init; }
    }
}