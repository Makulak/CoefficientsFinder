using System;
using CoefficientsFinder.Algorithm;

namespace CoefficientsFinder
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
            solver.IterationThreshold = 100000;
            solver.CreatePopulation();
            solver.PercentageMutationChance = 20;

            solver.Solve();
            Console.ReadKey();
        }
    }
}
