using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.Common.Exceptions
{
    public class DuplicateDataException : Exception
    {
        public DuplicateDataException() : base()
        {
        }

        public DuplicateDataException(string message) : base(message)
        {
        }

        public DuplicateDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
