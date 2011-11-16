using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public class Projectile : AnimatedGameComponent
    {
        

        public Projectile(EasyTopDownGame game, string sheetName)
            : base(game, sheetName)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            Update(gameTime);            
        }
    }
}
