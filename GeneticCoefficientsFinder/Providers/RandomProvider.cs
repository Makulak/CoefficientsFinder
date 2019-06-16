using System;

namespace CoefficientsFinder.Providers
{
    public class RandomProvider : IRandomProvider
    {
        private Random _random = new Random();

        public int Next()
        {
            return _random.Next(0,100);
        }

        public double NextDouble(int min)
        {
            return (_random.NextDouble()*_random.Next(1,100)) + min;
        }

        public int Next(int max)
        {
            return _random.Next(0, max);
        }
    }
}
