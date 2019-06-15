using System;

namespace AlgorytmGenetyczny.Providers
{
    public class RandomProvider : IRandomProvider
    {
        private Random _random = new Random();

        public int Next()
        {
            return Next(0, 100);
        }

        public int Next(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
