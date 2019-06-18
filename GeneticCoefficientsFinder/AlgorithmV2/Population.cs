using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using CoefficientsFinder.Providers;

namespace CoefficientsFinder.AlgorithmV2
{
    class Population
    {
        public List<Polynomial> List { get; private set; }

        public Polynomial BestFittedPolynomial => List.OrderBy(poly => poly).First();

        private List<Polynomial> _crossoveredChildren;
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
                throw new WrongValueException("Population count is less than 1");

            List.Clear();

            for (int i = 0; i < count; i++)
                List.Add(new Polynomial(_randomProvider, _degreeOfPolynomial));
        }

        public void CalculateFitness(List<Point> points)
        {
            foreach (var polynomial in List)
            {
                polynomial.CalculateFitness(points);
            }
        }

        public void Crossover()
        {
            _crossoveredChildren.Clear();

            for (int i = 0; i < List.Count - 1; ++i)
            {
                for (int j = i + 1; j < List.Count; ++j)
                {
                    CrossoverTwoPolynomials(List[i], List[j]);
                }
            }

            List.AddRange(_crossoveredChildren);
        }

        public void Mutate(int percentageChanceToMutation)
        {
            foreach (var polynomial in List)
            {
                polynomial.Mutate(percentageChanceToMutation);
            }
        }

        public void Cut(int survivalCount)
        {
            List.OrderBy(poly => poly).ToList().RemoveRange(survivalCount, List.Count - survivalCount);
        }

        private void CrossoverTwoPolynomials(Polynomial poly1, Polynomial poly2)
        {
            int cutPosition = _randomProvider.Next((_degreeOfPolynomial + 1) * sizeof(double) * 8);

            BitArray bytesOne = poly1.GetCoefficients();
            BitArray bytesTwo = poly2.GetCoefficients();

            for (int i = 0; i < cutPosition; ++i)
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
