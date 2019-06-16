namespace CoefficientsFinder.Providers
{
    public interface IRandomProvider
    {
        int Next();
        int Next(int max);
        double NextDouble(int min);
    }
}
