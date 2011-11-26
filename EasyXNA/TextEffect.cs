using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public class TextEffect : DrawableGameComponent
    {        
        public string BackgroundImageName { get; set; }        
        protected SpriteFont spriteFont;
        protected EasyTopDownGame game;
        protected string fontName;
        public Color FontColor {get;set;}                        
        public Color FlashColor {get;set;}
        public double FlashInterval {get;set;}
        public double lastFlashChange = -1;
        public double flashChangeCount = 0;
        public Vector2 Position;
        public float Scale;
        public String Message { get; set; }
        public double creationTime =-1;
        public double SecondsToLive { get; set; }
        public float LayerDepth { get; set; }

        public TextEffect(EasyTopDownGame game, string fontName)
            : base(game)
        {
            this.game = game;
            FontColor = Color.White;
            this.FontName = fontName;
            this.Scale = 1;            
            this.SecondsToLive = -1;
            this.Message = "";
            FlashColor = FontColor;
            FlashInterval = -1;
            LayerDepth = LayerDepths.Front;
        }


        public TextEffect(EasyTopDownGame game, string fontName, string message) : base(game)
        {
            this.game = game;
            FontColor = Color.White;
            this.FontName = fontName;
            this.Scale = 1;
            this.Message = message;
            this.SecondsToLive = -1;
            FlashColor = FontColor;
            FlashInterval = -1;
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

        public void SetPosition(int x, int y)
        {
            this.Position = new Vector2(x, y);
        }

        public override void Update(GameTime gameTime)
        {
            if (creationTime == -1)
            {
                creationTime = gameTime.TotalGameTime.TotalSeconds;
            }
            else
            {
                if (SecondsToLive >= 0)
                {
                    if (gameTime.TotalGameTime.TotalSeconds - creationTime >= SecondsToLive)
                    {
                        game.RemoveComponent(this);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Color currentColor = GetCurrentColor(gameTime);

            base.Draw(gameTime);            
            Vector2 fontOrigin = spriteFont.MeasureString(Message) / 2;            
            game.SpriteBatch.DrawString(spriteFont, Message, Position, currentColor, 0, fontOrigin, Scale, SpriteEffects.None, LayerDepth);            
        }

        public void MakeFlashingText(Color flashColor, double flashInterval)
        {
            this.FlashColor = flashColor;
            this.FlashInterval = flashInterval;
        }

        protected Color GetCurrentColor(GameTime gameTime)
        {

            double totalSeconds = gameTime.TotalGameTime.TotalSeconds;
            if (FlashInterval > 0)
            {
                if (lastFlashChange == -1)
                {
                    lastFlashChange = totalSeconds;                    
                }
                else
                {
                    if (totalSeconds - lastFlashChange > FlashInterval)
                    {
                        lastFlashChange = totalSeconds;
                        flashChangeCount++;
                        if (flashChangeCount % 2 == 0)
                        {
                            return FontColor;
                        }
                        else
                        {
                            return FlashColor;
                        }
                    }
                }
                return FontColor;
            }
            else  //We're not flashing at all
            {
                return FontColor;
            }
        }
    }
}
