using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyXNA
{
    public static class RandomHelper
    {
        static Random random = new Random();
        public static int IntInRange(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public static double DoubleInRange(double minValue, double maxValue)
        {
            double zeroToOne = random.NextDouble();
            //Min + (int)(Math.random() * ((Max - Min) + 1))
            double inRange = minValue + (zeroToOne * ((maxValue - minValue)));
            return inRange;
        }
    }
}
