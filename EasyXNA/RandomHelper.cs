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
            return random.Next(minValue, maxValue + 1);
        }

        public static double DoubleInRange(double minValue, double maxValue)
        {
            double zeroToOne = random.NextDouble();            
            double inRange = minValue + (zeroToOne * ((maxValue - minValue)));
            return inRange;
        }

        public static string PickOne(params string[] items)
        {
            int index = IntInRange(0, items.Length - 1);
            return items[index];
        }

    }
}
