using System;

namespace HotPotato.Core
{
    public static class Exceptions
    {
        public static ArgumentNullException ArgumentNull(string paramName) =>
            new ArgumentNullException(paramName);
        public static InvalidOperationException InvalidOperation(string message) =>
            new InvalidOperationException(message);
    }
}
