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


        public ScreenHelper(Rectangle titleSafeArea)
        {
            this.TitleSafeArea = titleSafeArea;
            center = new Vector2(TitleSafeArea.Center.X, TitleSafeArea.Center.Y);
            centerLeftThird = new Vector2(titleSafeArea.Width / 3 + titleSafeArea.X, titleSafeArea.Center.Y);
            centerRightThird = new Vector2(titleSafeArea.Width / 3 * 2 + titleSafeArea.X, titleSafeArea.Center.Y);
            centerLeftQuarter = new Vector2(titleSafeArea.Width / 4 + titleSafeArea.X, titleSafeArea.Center.Y);
            centerRightQuarter = new Vector2((titleSafeArea.Width / 4) * 3 + titleSafeArea.X, titleSafeArea.Center.Y);

        }

        public Vector2 Center { get { return center; } }

        public Vector2 CenterLeftThird { get { return centerLeftThird; } }
        public Vector2 CenterRightThird { get { return centerRightThird; } }
        public Vector2 CenterLeftQuarter { get { return centerLeftQuarter; } }
        public Vector2 CenterRightQuarter { get { return centerRightQuarter; } }

    }
}
