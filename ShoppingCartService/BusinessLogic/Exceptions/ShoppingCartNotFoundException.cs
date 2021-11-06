using System;
using System.Runtime.Serialization;

namespace ShoppingCartService.BusinessLogic.Exceptions
{
    [Serializable]
    public class ShoppingCartNotFoundException : Exception
    {
        public ShoppingCartNotFoundException()
        {
        }

        public ShoppingCartNotFoundException(string message) : base(message)
        {
        }

        public ShoppingCartNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ShoppingCartNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}