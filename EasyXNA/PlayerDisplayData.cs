using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public class PlayerDisplayData : DisplayData
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public PlayerDisplayData(EasyTopDownGame game, PlayerIndex playerIndex, string fontName) :base(game, fontName)
        {
            Name = "Player " + playerIndex.ToString();
            Score = 0;
            Scale = 1.0f;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            String displayData = Name + " : " + Score.ToString();
            Vector2 FontOrigin = spriteFont.MeasureString(displayData) / 2;
            game.SpriteBatch.Begin();
            game.SpriteBatch.DrawString(spriteFont, displayData, Position, FontColor, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
            game.SpriteBatch.End();
        }
    }
}
