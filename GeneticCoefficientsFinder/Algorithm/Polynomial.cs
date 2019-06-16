using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using CoefficientsFinder.Extensions;
using CoefficientsFinder.Providers;

namespace CoefficientsFinder.Algorithm
{
    public class Polynomial
    {
        public List<double> Coefficients { get; private set; }

        public double Fitness { get; set; }

        private int _degree;
        private IRandomProvider _randomProvider;

        public Polynomial(IRandomProvider randomProvider, int degreeOfPolynomial, BitArray coefficients)
        {
            if (degreeOfPolynomial < 0)
                throw new WrongValueException("Degree of polynomial is less than 0");

            Coefficients = new List<double>(degreeOfPolynomial + 1);

            _degree = degreeOfPolynomial;
            _randomProvider = randomProvider;

            ConvertByteArrayToCoefficients(coefficients);
        }

        public Polynomial(IRandomProvider randomProvider, int degreeOfPolynomial)
        {
            if (degreeOfPolynomial < 0)
                throw new WrongValueException("Degree of polynomial is less than 0");

            Coefficients = new List<double>(degreeOfPolynomial + 1);

            for (int i = 0; i < degreeOfPolynomial + 1; i++)
            {
                Coefficients.Add(randomProvider.Next());
            }

            _degree = degreeOfPolynomial;
            _randomProvider = randomProvider;
        }

        public void Mutate(int percentageChance)
        {
            BitArray bytes = GetAllCoefficientsInBytes();

            for (int i = 0; i < bytes.Length; i++)
            {
                if (_randomProvider.Next(0, 100) < percentageChance)
                {
                    if (bytes.Get(i))
                        bytes[i] = false;
                    else
                        bytes[i] = true;
                }
            }

            ConvertByteArrayToCoefficients(bytes);
        }

        public BitArray GetAllCoefficientsInBytes()
        {
            BitArray bytes = new BitArray(0);

            for (int i = 0; i < Coefficients.Count; i++)
                 bytes = bytes.Append(GetCoefficient(i));

            return bytes;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = Coefficients.Count - 1; i >= 0; i--)
            {
                if (i != 0)
                {
                    builder.Append($"{Coefficients[i]}x^{i} + ");
                }
                else
                {
                    builder.Append($"{Coefficients[i]}");
                }
            }

            return builder.ToString();
        }

        public double GetValue(double x)
        {
            //Horner's method to calculate value

            Coefficients.Reverse(); //TODO: Change that!

            var result = Coefficients.Aggregate(
                (accumulator, coefficient) => accumulator * x + coefficient);

            Coefficients.Reverse();

            return result;
        }

        public BitArray GetCoefficient(int degree)
        {
            return new BitArray(BitConverter.GetBytes(Coefficients[degree]));
        }

        public void ConvertByteArrayToCoefficients(BitArray array)
        {
            Coefficients.Clear();

            for (int i = 0; i < _degree + 1; i++)
            {
                var arr = array.ToByteArray();
                double value = BitConverter.ToDouble(array.ToByteArray(), i * sizeof(double));

                if (double.IsNaN(value))
                    return;
                Coefficients.Add(value);
            }
        }
    }
}
