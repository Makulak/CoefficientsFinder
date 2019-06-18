using System;
using CoefficientsFinder.Algorithm;

namespace CoefficientsFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Solver solver = new Solver();

            solver.AddPoint(-1,4);
            solver.AddPoint(0,1);
            solver.AddPoint(4,9);

            solver.SetMinimumRequiredDegreeOfPolynomial();
            solver.StartPopulationCount = 400;
            solver.SurvivalCount = 20;
            solver.IterationThreshold = 100000;
            solver.ExpectedDegreeOfPolynomial = 2;
            solver.PercentageMutationChance = 10;
            solver.CreatePopulation();
            Console.WriteLine("PopulationCreated\n=======================START========================\n");

            solver.Solve();
            Console.ReadKey();
        }
    }
}
