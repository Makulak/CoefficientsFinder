namespace AlgorytmGenetyczny.Providers
{
    public interface IRandomProvider
    {
        int Next();
        int Next(int min, int max);
    }
}
