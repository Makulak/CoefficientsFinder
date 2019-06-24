using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CoefficientsFinder.Extensions;
using CoefficientsFinder.Providers;

namespace CoefficientsFinder.AlgorithmV2
{
    class Polynomial : IComparable
    {
        public float? Fitness { get; set; }

        private List<float> _coefficients;
        private int _degree;
        private IRandomProvider _randomProvider;

        public Polynomial(IRandomProvider randomProvider, int degreeOfPolynomial, BitArray coefficients)
        {
            if (degreeOfPolynomial < 0)
                throw new WrongValueException("Degree of polynomial is less than 0");

            _coefficients = new List<float>(degreeOfPolynomial + 1);

            _degree = degreeOfPolynomial;
            _randomProvider = randomProvider;

            ConvertBitArrayToCoefficients(coefficients);
        }

        public Polynomial(IRandomProvider randomProvider, int degreeOfPolynomial)
        {
            if (degreeOfPolynomial < 0)
                throw new WrongValueException("Degree of polynomial is less than 0");

            _coefficients = new List<float>(degreeOfPolynomial + 1);

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

            for (int i = 0; i < bytes.Length; ++i)
            {
                if (_randomProvider.Next(100) < percentageChance)
                    bytes[i] = !bytes.Get(i);
            }

            ConvertBitArrayToCoefficients(bytes);
        }

        public BitArray GetCoefficients()
        {
            BitArray bytes = new BitArray(0);

            for (int i = 0; i < _coefficients.Count; ++i)
                bytes = bytes.Append(GetCoefficient(i));

            return bytes;
        }

        public void ConvertBitArrayToCoefficients(BitArray array)
        {
            _coefficients.Clear();

            for (int i = 0; i <= _degree; ++i)
            {
                float value = BitConverter.ToSingle(array.ToByteArray(), i * sizeof(float));
                //value = (float)Math.Round(value, 2);
                while (float.IsNaN(value))
                {
                    value = FixNaN(array, i);
                }

                _coefficients.Add(value);
            }
        }

        public void CalculateFitness(List<Point> points)
        {
            float error = 0;
            foreach (var point in points)
            {
                float val = GetValue(point.X);
                error += Math.Abs(point.Y - val);
            }

            Fitness = error;
        }

        public float GetValue(float x)
        {
            float result = _coefficients[_coefficients.Count - 1];

            for (int i = _coefficients.Count - 2; i >= 0; --i)
                result = result * x + _coefficients[i];

            return result;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = _coefficients.Count - 1; i >= 0; i--)
                builder.Append(i != 0 ? $"{_coefficients[i]}x^{i} + " : $"{_coefficients[i]}");

            return builder.ToString();
        }

        public int CompareTo(object obj)
        {
            Polynomial p = obj as Polynomial;

            if (p == null)
                throw new WrongValueException("Wrong object to compare!");
            if (p.Fitness == null || Fitness == null)
            {
                if (p.Fitness == null && Fitness == null)
                    return 0;
                if (p.Fitness == null && Fitness != null)
                    return -1;
                return 1;
            }

            if (Fitness > p.Fitness)
                return 1;
            if (Fitness.Equals(p.Fitness))
                return 0;
            return -1;
        }

        private BitArray GetCoefficient(int degree)
        {
            if (degree < 0 || degree >= _coefficients.Count)
                throw new WrongValueException("Wrong degree!");

            return new BitArray(BitConverter.GetBytes(_coefficients[degree]));
        }

        private float FixNaN(BitArray array, int number)
        {
            //This method changes random bit in Exponent to '0'
            int randomIndex = (number + 1) * sizeof(float) * 8 - _randomProvider.Next(8);
            array.Set(randomIndex - 1, false);
            return BitConverter.ToSingle(array.ToByteArray(), number * sizeof(float));
        }
    }
}
