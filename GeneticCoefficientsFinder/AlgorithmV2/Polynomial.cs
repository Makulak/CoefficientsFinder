using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoefficientsFinder.Providers;

namespace CoefficientsFinder.AlgorithmV2
{
    class Polynomial
    {
        public double Fitness { get; set; }

        private List<double> _coefficients;
        private int _degree;
        private IRandomProvider _randomProvider;

        public Polynomial(IRandomProvider randomProvider, int degreeOfPolynomial, BitArray coefficients)
        {
            if (degreeOfPolynomial < 0)
                throw new WrongValueException("Degree of polynomial is less than 0");

            _coefficients = new List<double>(degreeOfPolynomial + 1);

            _degree = degreeOfPolynomial;
            _randomProvider = randomProvider;

            ConvertByteArrayToCoefficients(coefficients);
        }

        public Polynomial(IRandomProvider randomProvider, int degreeOfPolynomial)
        {
            if (degreeOfPolynomial < 0)
                throw new WrongValueException("Degree of polynomial is less than 0");

            _coefficients = new List<double>(degreeOfPolynomial + 1);

            _degree = degreeOfPolynomial;
            _randomProvider = randomProvider;

            for (int i = 0; i < degreeOfPolynomial + 1; ++i)
            {
                _coefficients.Add(randomProvider.Next());
            }
        }

        public void Mutate(int percentageChance)
        {
            BitArray bytes = GetCoefficients();

            for (int i = 0; i < bytes.Length; i++)
            {
                if (_randomProvider.Next(100) < percentageChance)
                {
                    if (bytes.Get(i))
                        bytes[i] = false;
                    else
                        bytes[i] = true;
                }
            }

            ConvertBitArrayToCoefficients(bytes);
        }

        public BitArray GetCoefficients()
        {

        }
    }
}
