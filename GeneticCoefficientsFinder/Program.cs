using System;

namespace CoefficientsFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            //StartSolver1();
            StartSolver2();
        }

        public static void StartSolver1()
        {
            Algorithm.Solver solver = new Algorithm.Solver();

            solver.AddPoint(-1, 4);
            solver.AddPoint(0, 1);
            solver.AddPoint(4, 9);

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

        public static void StartSolver2()
        {
            AlgorithmV2.Solver solver = new AlgorithmV2.Solver();

            solver.AddPoint(-1, 4);
            solver.AddPoint(2, 5);
            solver.AddPoint(4, 9);

            solver.SetMinimumRequiredDegreeOfPolynomial();
            solver.StartPopulationCount = 400;
            solver.SurvivalCount = 20;
            solver.IterationThreshold = 1000000;
            solver.ExpectedDegreeOfPolynomial = 7;
            solver.PercentageMutationChance = 10;
            solver.RequiredPolynomialFitness = 0.05;
            solver.CreatePopulation();
            Console.WriteLine("PopulationCreated\n=======================START========================\n");

            solver.Solve();
            Console.ReadKey();
        }
    }
}
