using System;

namespace CSP.Exceptions
{
    public class SolutionNotFoundException : Exception
    {
        public SolutionNotFoundException()
        {
        }

        public SolutionNotFoundException(string message) : base(message)
        {
        }
    }
}