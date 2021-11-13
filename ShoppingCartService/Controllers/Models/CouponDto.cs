using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartService.Controllers.Models
{
    public abstract record CouponDto
    {
        [Required] public string Id { get; init; }

        [Required] public DateTime ExpiredAt { get; init; }
    }

    public record FreeShippingCouponDto : CouponDto{}

    public record TypeAbsoluteCouponDto : CouponDto
    {
        [Required] [ Range(0, double.MaxValue)] public double Amount { get; init; }
    }

    public record TypePercentageCouponDto : CouponDto
    {
        [Required] [Range(0, 100)] public double Percentage { get; init; }
    }
}