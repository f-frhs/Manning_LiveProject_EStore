using System;
using System.Runtime.Serialization;

namespace ShoppingCartService.BusinessLogic.Exceptions
{
    [Serializable]
    public class ItemNotFoundInCartException : Exception
    {
        public ItemNotFoundInCartException()
        {
        }

        public ItemNotFoundInCartException(string message) : base(message)
        {
        }

        public ItemNotFoundInCartException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ItemNotFoundInCartException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}