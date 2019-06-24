using System;
using System.Collections.Generic;
using System.Linq;
using CoefficientsFinder.Providers;

namespace CoefficientsFinder.Algorithm
{
    [Obsolete]
    public class Solver
    {
        public List<Tuple<double, double>> Points { get; }

        public int IterationThreshold { get; set; }
        public int SurvivalCount { get; set; }
        public int StartPopulationCount { get; set; }
        public int PercentageMutationChance { get; set; }
        public int ExpectedDegreeOfPolynomial { get; set; }
        public double CompareMargin { get; set; }

        public Population Population { get; set; }

        public Solver()
        {
            Points = new List<Tuple<double, double>>();

            CompareMargin = 0.001;
        }

        public void AddPoint(Tuple<double, double> point)
        {
            if (Points.Any(p => Math.Abs(p.Item1 - point.Item1) < CompareMargin))
                throw new WrongValueException("X values can not be repeated");

            Points.Add(point);
        }

        public void AddPoint(double x, double y)
        {
            if (Points.Any(p => Math.Abs(p.Item1 - x) < CompareMargin))
                throw new WrongValueException("X values can not be repeated");

            Points.Add(new Tuple<double, double>(x, y));
        }

        public int GetMinimumDegreeOfPolynomial()
        {
            var temp = Points.OrderBy(point => point.Item1).ToArray();
            int minDegree = 0;

            for (int i = 0; i < temp.Length - 1; i++)
            {
                bool lastSign = temp[i].Item2 > 0;

                for (int j = i + 1; j < temp.Length; j++)
                {
                    if (temp[j].Item2 > 0 != lastSign)
                    {
                        ++minDegree;
                        i = j - 1;
                        break;
                    }
                }
            }

            if (minDegree == 0 && temp.Any(point => point.Item2 != temp[0].Item2))
                minDegree++;

            return minDegree;
        }

        public void SetMinimumRequiredDegreeOfPolynomial()
        {
            ExpectedDegreeOfPolynomial = GetMinimumDegreeOfPolynomial();
        }

        public void CreatePopulation()
        {
            if (StartPopulationCount < 1)
                throw new WrongValueException("Start population count is less than 1");
            if (Points == null || Points.Count == 0)
                throw new WrongValueException("Points list count is empty");
            if (ExpectedDegreeOfPolynomial < 0)
                throw new WrongValueException("Degree of polynomial is less than 0");
            if (ExpectedDegreeOfPolynomial < GetMinimumDegreeOfPolynomial())
                throw new WrongValueException("Degree of polynomial is less than minimum required degree");

            Population = new Population(new RandomProvider(), ExpectedDegreeOfPolynomial);

            Population.Create(StartPopulationCount);
        }

        public void Solve()
        {
            if (IterationThreshold < 1)
                throw new WrongValueException("Iteration threshold is less than 1");
            if (SurvivalCount < 1)
                throw new WrongValueException("Survival count is less than 1");
            if (StartPopulationCount < 1)
                throw new WrongValueException("Start population count is less than 1");
            if (PercentageMutationChance < 0)
                throw new WrongValueException("Mutation chance is less than 0");
            if (ExpectedDegreeOfPolynomial < 0)
                throw new WrongValueException("Degree of polynomial is less than 1");
            if (ExpectedDegreeOfPolynomial < GetMinimumDegreeOfPolynomial())
                throw new WrongValueException("Degree of polynomial is less than minimum required degree");
            if (Population == null || Population.List.Count == 0)
                throw new WrongValueException("Start population count is less than 1");

            for (int i = 0; i < IterationThreshold; i++)
            {
                Population.CalculateFitness(Points);
                if (i % 300 == 0)
                    WriteInfo();
                Population.Cut(SurvivalCount);
                Population.Crossover();
                Population.Mutate(PercentageMutationChance);

                if (Population.FitnessForBestPolynomial(Points) < 0.03)
                {
                    Console.WriteLine();
                    Console.WriteLine("==============================SUCCESS==============================");
                    Console.WriteLine($"============================ITERATION: {i}=========================");
                    WriteInfo();
                    return;
                }
            }
        }

        private void WriteInfo()
        {
            var t = Population.BestFittedPolynomial;

            Console.WriteLine($"Searched values:");
            foreach (var point in Points)
            {
                Console.WriteLine($"X: {point.Item1} Y: {point.Item2}");
            }

            Console.WriteLine($"Best fitted values:");
            foreach (var point in Points)
            {
                Console.WriteLine($"X: {point.Item1} Y: {t.GetValue(point.Item1)}");
            }

            Console.WriteLine("Polynomial:");
            Console.WriteLine(t.ToString());
        }
    }
}
