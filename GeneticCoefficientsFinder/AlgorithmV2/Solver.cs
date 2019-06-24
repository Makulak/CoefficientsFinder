using System;
using System.Collections.Generic;
using System.Linq;
using CoefficientsFinder.Providers;

namespace CoefficientsFinder.AlgorithmV2
{
    public class Solver
    {
        public List<Point> Points { get; set; }

        public int IterationThreshold {
            get => _iterationThreshold;
            set => _iterationThreshold = value < 1 ? 1 : value;
        }
        private int _iterationThreshold;

        public int SurvivalCount {
            get => _survivalCount;
            set => _survivalCount = value < 1 ? 1 : value;
        }
        private int _survivalCount;

        public int StartPopulationCount {
            get => _startPopulationCount;
            set => _startPopulationCount = value < 1 ? 1 : value;
        }
        private int _startPopulationCount;

        public int PercentageMutationChance {
            get => _percentageMutationChance;
            set {
                if (value > 100)
                    _percentageMutationChance = 100;
                else if (value < 0)
                    _percentageMutationChance = 0;
                else
                    _percentageMutationChance = value;
            }
        }
        private int _percentageMutationChance;

        public int ExpectedDegreeOfPolynomial {
            get => _expectedDegreeOfPolynomial;
            set {
                if (value < 0)
                    _expectedDegreeOfPolynomial = 0;
                else if (value > 10)
                    _expectedDegreeOfPolynomial = 10;
                else
                    _expectedDegreeOfPolynomial = value;
            }
        }
        private int _expectedDegreeOfPolynomial;

        public double RequiredPolynomialFitness { get; set; }

        private Population _population;

        public Solver()
        {
            Points = new List<Point>();

            //Default values
            IterationThreshold = 1000;
            SurvivalCount = 5;
            StartPopulationCount = 25;
            PercentageMutationChance = 10;
        }

        public void AddPoint(float x, float y)
        {
            AddPoint(new Point(x, y));
        }

        public void AddPoint(Point point)
        {
            if (Points.Any(p => p == point))
                throw new WrongValueException("X values can not be repeated!");

            Points.Add(point);
        }

        public int GetMinimumRequiredDegreeOfPolynomial()
        {
            int minimumDegree = 0;
            Points = Points.OrderBy(point => point).ToList();

            for (int i = 0; i < Points.Count; ++i)
            {
                if (Points.Skip(i).Any(point => !point.Y.Equals(Points[i].Y)))
                    minimumDegree++;
            }

            return minimumDegree;
        }

        public void SetMinimumRequiredDegreeOfPolynomial()
        {
            ExpectedDegreeOfPolynomial = GetMinimumRequiredDegreeOfPolynomial();
        }

        public void CreatePopulation()
        {
            _population = new Population(new RandomProvider(), ExpectedDegreeOfPolynomial);

            _population.Create(StartPopulationCount);
        }

        public bool Solve()
        {
            if (ExpectedDegreeOfPolynomial < GetMinimumRequiredDegreeOfPolynomial())
                throw new WrongValueException("Degree of polynomial is less than minimum required degree");

            for (int i = 0; i < IterationThreshold; ++i)
            {
                _population.CalculateFitness(Points);

                if (_population.BestFittedPolynomial.Fitness < RequiredPolynomialFitness)
                {
                    WriteInfo();
                    return true;
                }
                if(i%100 == 0)
                    WriteInfo();

                _population.Cut(SurvivalCount);
                _population.Crossover();
                _population.FillWithRandom(StartPopulationCount);
                _population.Mutate(PercentageMutationChance);
            }

            return false;
        }

        private void WriteInfo()
        {
            foreach (var point in Points)
            {
                Console.WriteLine($"X: {point.X}, Searched: {point.Y}, Found: {_population.BestFittedPolynomial.GetValue(point.X)}");
            }
            Console.WriteLine();
            Console.WriteLine($"Polynomial: {_population.BestFittedPolynomial.ToString()}");
            Console.WriteLine("====================================================");
        }
    }
}
