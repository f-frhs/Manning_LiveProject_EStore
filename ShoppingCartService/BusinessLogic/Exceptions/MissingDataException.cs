using System;
using System.Runtime.Serialization;

namespace ShoppingCartService.BusinessLogic.Exceptions
{
    [Serializable]
    public class MissingDataException : Exception
    {
        public MissingDataException()
        {
        }

        public MissingDataException(string message) : base(message)
        {
        }

        public MissingDataException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingDataException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}