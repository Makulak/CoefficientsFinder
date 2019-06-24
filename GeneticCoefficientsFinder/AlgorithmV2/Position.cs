using System;
using System.Collections;

namespace CoefficientsFinder.AlgorithmV2
{
    public class Point : IComparable
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Point p))
                throw new WrongValueException("Wrong object to compare!");

            return X.CompareTo(p.X);
        }
    }
}
