using System;

namespace HS.Message.Share.Handlers
{
    public class UserOperationException : Exception
    {
        public UserOperationException() { }

        public UserOperationException(string message) : base(message) { }

        public UserOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
