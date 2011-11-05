using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public abstract class DisplayData
    {

        
        public string BackgroundImageName { get; set; }
        protected string fontName;
        protected SpriteFont spriteFont;
        protected EasyTopDownGame game;
        protected Color FontColor;
        public Vector2 Position;
        public float Scale;

        public DisplayData(EasyTopDownGame game)
        {
            this.game = game;
            FontColor = Color.White;
            this.FontName = game.PlayerFontName;
            this.Scale = 1;
        }

        public string FontName
        {
            get
            {
                return fontName;
            }
            set
            {
                this.fontName = value;
                this.spriteFont = game.Content.Load<SpriteFont>(fontName);
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public void SetPosition(int x, int y)
        {
            this.Position = new Vector2(x, y);
        }
    }
}
