using System;

namespace AlgorytmGenetyczny
{
    public class WrongValueException : Exception
    {
        public WrongValueException(string message) : base(message)
        { }
    }
}
