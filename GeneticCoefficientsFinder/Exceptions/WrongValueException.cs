using System;

namespace CoefficientsFinder
{
    public class WrongValueException : Exception
    {
        public WrongValueException(string message) : base(message)
        { }
    }
}
