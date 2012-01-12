using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        public static Color Color()
        {
            int red = IntInRange(0,255);
            int green = IntInRange(0,255);
            int blue = IntInRange(0,255);
            int alpha = IntInRange(0,255);


            return new Color(red, green, blue, alpha);
        }

    }
}
