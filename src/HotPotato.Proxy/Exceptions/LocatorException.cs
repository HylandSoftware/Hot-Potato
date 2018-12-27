using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HotPotato.Exceptions
{
    public class LocatorException : Exception
    {
        public LocatorException()
        {
        }

        public LocatorException(string message) : base(message)
        {
        }

        public LocatorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LocatorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
