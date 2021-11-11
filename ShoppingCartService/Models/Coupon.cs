using System;
using System.ComponentModel;

namespace ShoppingCartService.Models
{
    public class Coupon
    {
        public ECouponType Type { get; private set; }

        public double Amount { get; private set; }

        public double Percentage { get; private set; }

        public Coupon(double amount)
            : this(amount, ECouponType.Absolute)
        {
            setAmount(amount);
        }

        public Coupon(double val, ECouponType couponType)
        {
            if (couponType == ECouponType.Absolute)
            {
                setAmount(val);
                return;
            }

            if (couponType == ECouponType.Percentage)
            {
                setPercentage(val);
                return;
            }

            throw new InvalidEnumArgumentException(nameof(couponType), (int) couponType, typeof(ECouponType));
        }

        private void setAmount(double amount)
        {
            if (!(0 <= amount))
            {
                throw new ArgumentOutOfRangeException(nameof(amount),
                    $"{nameof(amount)} cannot be negative number.");
            }

            Type = ECouponType.Absolute;
            Amount = amount;
        }

        private void setPercentage(double value)
        {
            if (!(0 <= value))
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"{nameof(value)} cannot be negative number.");
            }

            if (!(value<=100))
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"{nameof(value)} cannot be more than 100.");
            }

            Type = ECouponType.Percentage;
            Percentage = value;
        }
    }

    public enum ECouponType
    {
        Absolute,
        Percentage,
    }
}