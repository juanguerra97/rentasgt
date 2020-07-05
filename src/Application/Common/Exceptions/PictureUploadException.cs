using System;

namespace rentasgt.Application.Common.Exceptions
{
    public class PictureUploadException : Exception
    {

        public PictureUploadException()
            : base()
        { }

        public PictureUploadException(string message)
            : base(message)
        { }

        public PictureUploadException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}
