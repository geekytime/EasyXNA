using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public class ScreenHelper
    {
        protected Rectangle TitleSafeArea { get; set; }
        private Vector2 center;
        private Vector2 centerLeftThird;
        private Vector2 centerRightThird;
        private Vector2 centerLeftQuarter;
        private Vector2 centerRightQuarter;
        private Vector2 topLeftThird;
        private Vector2 topRightThird;
        private Vector2 topLeftQuarter;
        private Vector2 topRightQuarter;

        public ScreenHelper(Rectangle titleSafeArea)
        {
            this.TitleSafeArea = titleSafeArea;
            center = new Vector2(TitleSafeArea.Center.X, TitleSafeArea.Center.Y);
            centerLeftThird = new Vector2(titleSafeArea.Width / 3 + titleSafeArea.X, titleSafeArea.Center.Y);
            centerRightThird = new Vector2(titleSafeArea.Width / 3 * 2 + titleSafeArea.X, titleSafeArea.Center.Y);
            centerLeftQuarter = new Vector2(titleSafeArea.Width / 4 + titleSafeArea.X, titleSafeArea.Center.Y);
            centerRightQuarter = new Vector2((titleSafeArea.Width / 4) * 3 + titleSafeArea.X, titleSafeArea.Center.Y);

            topLeftThird = new Vector2(titleSafeArea.Width / 3 + titleSafeArea.X, titleSafeArea.Top);
            topRightThird = new Vector2(titleSafeArea.Width / 3 * 2 + titleSafeArea.X, titleSafeArea.Top);
            topLeftQuarter = new Vector2(titleSafeArea.Width / 4 + titleSafeArea.X, titleSafeArea.Top);
            topRightQuarter = new Vector2((titleSafeArea.Width / 4) * 3 + titleSafeArea.X, titleSafeArea.Top);

        }

        public Vector2 Center { get { return center; } }

        public Vector2 CenterLeftThird { get { return centerLeftThird; } }
        public Vector2 CenterRightThird { get { return centerRightThird; } }
        public Vector2 CenterLeftQuarter { get { return centerLeftQuarter; } }
        public Vector2 CenterRightQuarter { get { return centerRightQuarter; } }

        public Vector2 TopLeftThird { get { return topLeftThird; } }
        public Vector2 TopRightThird { get { return topRightThird; } }
        public Vector2 TopLeftQuarter { get { return topLeftQuarter; } }
        public Vector2 TopRightQuarter { get { return topRightQuarter; } }
    }
}
