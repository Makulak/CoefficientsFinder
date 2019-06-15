using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using AlgorytmGenetyczny.Extensions;
using AlgorytmGenetyczny.Providers;

namespace AlgorytmGenetyczny.Algorithm
{
    public class Polynomial
    {
        public List<double> Coefficients { get; private set; }

        public double Fitness { get; set; }

        private int _degree;
        private IRandomProvider _randomProvider;

        public Polynomial(IRandomProvider randomProvider, int degreeOfPolynomial, byte[] coefficients)
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
            byte[] bytes = GetAllCoefficientsInBytes();

            for (int i = 0; i < bytes.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_randomProvider.Next(0, 100) < percentageChance)
                    {
                        bytes[i] = bytes[i].ToggleBit(j);
                    }
                }
            }

            ConvertByteArrayToCoefficients(bytes);
        }

        public byte[] GetAllCoefficientsInBytes()
        {
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < Coefficients.Count; i++)
                bytes.AddRange(GetCoefficient(i));

            return bytes.ToArray();
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

        public byte[] GetCoefficient(int degree)
        {
            return BitConverter.GetBytes(Coefficients[degree]);
        }

        public void ConvertByteArrayToCoefficients(byte[] array)
        {
            Coefficients.Clear();

            for (int i = 0; i < _degree + 1; i++)
            {
                double value = Math.Round(BitConverter.ToDouble(array, i * sizeof(double)), 3);
                Coefficients.Add(Math.Round(value,3));
            }
        }
    }
}
