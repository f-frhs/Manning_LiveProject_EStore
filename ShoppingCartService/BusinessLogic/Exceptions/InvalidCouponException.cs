using System;
using System.Runtime.Serialization;

namespace ShoppingCartService.BusinessLogic.Exceptions
{
    [Serializable]
    public class InvalidCouponException : Exception
    {
        public InvalidCouponException()
        {
        }

        public InvalidCouponException(string message) : base(message)
        {
        }

        public InvalidCouponException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidCouponException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}