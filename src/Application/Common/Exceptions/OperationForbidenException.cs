using System;

namespace rentasgt.Application.Common.Exceptions
{
    public class OperationForbidenException : Exception
    {

        public OperationForbidenException() : base()
        {
        }

        public OperationForbidenException(string message) : base(message)
        {
        }

        public OperationForbidenException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
