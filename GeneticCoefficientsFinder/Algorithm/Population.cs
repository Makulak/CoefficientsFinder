using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CoefficientsFinder.Extensions;
using CoefficientsFinder.Providers;

namespace CoefficientsFinder.Algorithm
{
    public class Population
    {
        public List<Polynomial> List { get; private set; }
        private List<Polynomial> _crossoveredChildren;

        public Polynomial BestFittedPolynomial => List.OrderBy(poly => poly.Fitness).First();

        public double FitnessForBestPolynomial(List<Tuple<double, double>> points) =>
            CalculateFitnessForPolynomial(points, BestFittedPolynomial);

        private readonly int _degreeOfPolynomial;
        private readonly IRandomProvider _randomProvider;

        public Population(IRandomProvider randomProvider, int degreeOfPolynomial)
        {
            if (degreeOfPolynomial < 0)
                throw new WrongValueException("Degree of polynomial is less than 0");

            List = new List<Polynomial>();
            _crossoveredChildren = new List<Polynomial>();

            _degreeOfPolynomial = degreeOfPolynomial;
            _randomProvider = randomProvider;
        }

        public void Create(int count)
        {
            if (count < 1)
                throw new WrongValueException("Count is less than 1");

            List.Clear();

            for (int i = 0; i < count; i++)
                List.Add(new Polynomial(_randomProvider, _degreeOfPolynomial));
        }

        public void OrderList()
        {
            List = List.OrderBy(poly => poly.Fitness).ToList();
        }

        public void CalculateFitness(List<Tuple<double, double>> points) //TODO: Tests
        {
            foreach (var polynomial in List)
            {
                polynomial.Fitness = CalculateFitnessForPolynomial(points, polynomial);
            }
        }

        public void Crossover() //TODO: Tests
        {
            _crossoveredChildren.Clear();
            for (int i = 0; i < List.Count - 1; i++)
            {
                for (int j = i + 1; j < List.Count; j++)
                {
                    CrossoverSingle(List[i], List[j]);
                }
            }
            List.AddRange(_crossoveredChildren);
        }

        public void Cut(int survivalCount) //TODO: Tests
        {
            OrderList();
            List.RemoveRange(survivalCount, List.Count - survivalCount);
        }

        public void Mutate(int percentageChanceToMutation)
        {
            foreach (var polynomial in List)
            {
                polynomial.Mutate(percentageChanceToMutation);
            }
        }

        public double CalculateFitnessForPolynomial(List<Tuple<double, double>> points, Polynomial polynomial)
        {
            double error = 0;
            foreach (var point in points)
            {
                double val = polynomial.GetValue(point.Item1);
                error += Math.Abs(point.Item2 - val);
            }

            return error;
        }

        private void CrossoverSingle(Polynomial poly1, Polynomial poly2)
        {
            int cutPosition = _randomProvider.Next(1, (_degreeOfPolynomial + 1) * sizeof(double) * 8);

            BitArray bytesOne = poly1.GetAllCoefficientsInBytes();
            BitArray bytesTwo = poly2.GetAllCoefficientsInBytes();

            for (int i = bytesOne.Length-1; i >= cutPosition; i--)
            {
                bool bitOne = bytesOne[i];
                bool bitTwo = bytesTwo[i];

                if (bitOne != bitTwo)
                {
                    bytesOne[i] = bitTwo;
                    bytesTwo[i] = bitOne;
                }
            }

            _crossoveredChildren.Add(new Polynomial(_randomProvider, _degreeOfPolynomial, bytesOne));
            _crossoveredChildren.Add(new Polynomial(_randomProvider, _degreeOfPolynomial, bytesTwo));
        }
    }
}
