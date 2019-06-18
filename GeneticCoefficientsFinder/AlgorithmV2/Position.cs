using System.Collections;

namespace CoefficientsFinder.AlgorithmV2
{
    public class Point : IComparer
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public int Compare(object a, object b)
        {
            Point p1 = a as Point;
            Point p2 = b as Point;

            if(p1 == null || p2 == null)
                throw new WrongValueException("Wrong object to compare!");

            return p1.X.CompareTo(p2.X);
        }
    }
}
