using System;

namespace CSP.Exceptions
{
    public class FileAlreadyExistsException : Exception
    {
        public FileAlreadyExistsException()
        {
        }

        public FileAlreadyExistsException(string message) : base(message)
        {
        }
    }
}