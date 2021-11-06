namespace ShoppingCartService.Models
{
    /// <summary>
    /// Shipping method
    /// </summary>
    public enum ShippingMethod
    {
        /// <summary>
        /// Slow
        /// </summary>
        Standard,

        /// <summary>
        /// Slow, but make an effort
        /// </summary>
        Expedited,

        /// <summary>
        /// I need it as soon as possible
        /// </summary>
        Priority,

        /// <summary>
        /// I will pay to have it here tomorrow!
        /// </summary>
        Express
    }
}