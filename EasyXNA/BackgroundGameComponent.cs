using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EasyXNA
{
    public class BackgroundGameComponent : DrawableGameComponent
    {
        private string imageName;
        private Rectangle tileArea;
        private EasyTopDownGame game;
        private Texture2D texture;
        private int tileRows;
        private int tileCols;
        private Vector2 scalingFactor;

        public BackgroundGameComponent(EasyTopDownGame game, string imageName, Rectangle tileArea) : base(game)
        {
            this.game = game;
            this.imageName = imageName;
            this.tileArea = tileArea;
            this.DrawOrder = 0;
            texture = game.Content.Load<Texture2D>(imageName);
            CalculateScalingFactor();
            
        }

        private void CalculateScalingFactor()
        {
            tileCols = tileArea.Width / texture.Width;
            tileRows = tileArea.Height / texture.Height;

            float scalingFactorX = (float)tileArea.Width / (float)tileCols / (float)texture.Width;
            float scalingFactorY = (float)tileArea.Height / (float)tileRows / (float)texture.Height;
            scalingFactor = new Vector2(scalingFactorX, scalingFactorY);
        }

        public override void Draw(GameTime gameTime)
        {
            game.SpriteBatch.Begin();
            for (int row = 0; row < tileRows; row++)
            {
                for(int col = 0; col<tileCols; col++)
                {
                    float x = tileArea.Left + ((col * texture.Width) * scalingFactor.X);
                    float y = tileArea.Top + ((row * texture.Height) * scalingFactor.Y);
                    Vector2 position = new Vector2(x,y);
                    game.SpriteBatch.Draw(texture,position, null, Color.White,0, Vector2.Zero, scalingFactor, SpriteEffects.None, 1);
                }
            }            
            game.SpriteBatch.End();
        }
    }
}
