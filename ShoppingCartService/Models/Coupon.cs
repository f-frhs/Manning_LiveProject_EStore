namespace ShoppingCartService.Models
{
    public class Coupon
    {
        public uint Amount { get; }

        public Coupon(uint amount)
        {
            Amount = amount;
        }
    }
}