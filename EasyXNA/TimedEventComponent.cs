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

        public TimedEventComponent(EasyTopDownGame game, double interval, Action callback)
            : base(game)
        {
            this.interval = interval;
            this.callback = callback;
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
                callback();
                lastCall = totalSeconds;
            }

            base.Update(gameTime);
        }
    }
}
