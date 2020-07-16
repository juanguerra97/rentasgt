using System;

namespace rentasgt.Application.Common.Exceptions
{
    public class InvalidRentRequestException : Exception
    {
        public InvalidRentRequestException() : base()
        {
        }

        public InvalidRentRequestException(string message) : base(message)
        {
        }

        public InvalidRentRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
