using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpriteSheetRuntime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EasyXNA
{
    public class EffectGameComponent : DrawableGameComponent
    {
        SpriteSheet SpriteSheet { get; set; }
        String ImagePrefix {get;set;}
        protected double SecondsPerFrame {get;set;}
        protected EasyTopDownGame game;
        public int CurrentFrame { get; set; }
        public int MaxLoops { get; set; }
        private Vector2 position;
        public Color OverlayColor { get; set; }
        private Vector2 offset;
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public float LayerDepth { get; set; }
        private double lastFrameChange = -1;
        private int loopCount = 0;

        public EffectGameComponent(EasyTopDownGame game, String sheetName)
            : base(game)
        {
            SpriteSheet = game.Content.Load<SpriteSheet>(sheetName);
            this.game = game;
            ImagePrefix = sheetName;
            CurrentFrame = 0;
            SecondsPerFrame = 0.1;
            MaxLoops = 1;
            Position = new Vector2(0, 0);
            OverlayColor = Color.White;
            Rotation = 0;
            Scale = 1;
            LayerDepth = 0;
        }        

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {                
                position = value;
            }
        }

        public void SetPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        protected String GetCurrentFrameName()
        {
            String currentFrameName = ImagePrefix + "-" + CurrentFrame;
            return currentFrameName;
        }

        public override void Draw(GameTime gameTime)
        {            
            Rectangle sourceRectangle = SpriteSheet.SourceRectangle(GetCurrentFrameName());            
            int offsetX = sourceRectangle.Width /2;
            int offsetY = sourceRectangle.Height /2;
            Vector2 offset = new Vector2(offsetX, offsetY);

            game.SpriteBatch.Begin();
            game.SpriteBatch.Draw(SpriteSheet.Texture, position, sourceRectangle, OverlayColor, Rotation, offset, Scale,  SpriteEffects.None, LayerDepth);
            game.SpriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            ClickAnimationFrame(gameTime);
            base.Update(gameTime);
        }

        protected virtual void ClickAnimationFrame(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - lastFrameChange > SecondsPerFrame)
            {
                lastFrameChange = gameTime.TotalGameTime.TotalSeconds;
                if (CurrentFrame == SpriteSheet.Count - 1)
                {
                    loopCount++;
                    if (loopCount >= MaxLoops)
                    {
                        game.Components.Remove(this);
                    }
                    else
                    {
                        CurrentFrame = 0;
                    }
                }
                else
                {
                    CurrentFrame++;
                }
            }            
        }
    }
}
