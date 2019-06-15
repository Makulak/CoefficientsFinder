using System;
using AlgorytmGenetyczny.Algorithm;

namespace AlgorytmGenetyczny
{
    class Program
    {
        static void Main(string[] args)
        {
            Solver solver = new Solver();

            solver.AddPoint(1,2);
            solver.AddPoint(2,3);

            solver.SetMinimumRequiredDegreeOfPolynomial();
            solver.StartPopulationCount = 400;
            solver.SurvivalCount = 20;
            solver.IterationThreshold = 100;
            solver.CreatePopulation();
            solver.PercentageMutationChance = 10;

            solver.Solve();
            Console.ReadKey();
        }
    }
}
