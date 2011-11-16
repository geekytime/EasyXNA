using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public class TimedEventComponent : GameComponent
    {
        double interval;
        Action callback;
        double lastCall = -1;
        int maxCount = -1;
        int count = 0;

        public TimedEventComponent(EasyTopDownGame game, double interval, Action callback)
            : base(game)
        {
            this.interval = interval;
            this.callback = callback;
        }

        public TimedEventComponent(EasyTopDownGame game, double interval, Action callback, int maxCount)
            : base(game)
        {
            this.interval = interval;
            this.callback = callback;
            this.maxCount = maxCount;
        }

        public override void Update(GameTime gameTime)
        {
            double totalSeconds = gameTime.TotalGameTime.TotalSeconds;
            if (lastCall == -1)
            {
                lastCall = totalSeconds;
                return;
            }

            if (totalSeconds - lastCall > interval)
            {
                count++;                
                callback();
                lastCall = totalSeconds;
                if (count == maxCount)
                {
                    base.Game.Components.Remove(this);
                }
            }


            base.Update(gameTime);
        }
    }
}
