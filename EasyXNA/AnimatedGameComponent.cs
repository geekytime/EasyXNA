using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SpriteSheetRuntime;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace EasyXNA
{
    public class AnimatedGameComponent : EasyGameComponent
    {
        protected SpriteSheet spriteSheet;        
        protected int framesPerDirection = 2;
        protected int currentAnimationFrame = 0;

        protected double lastFrameChange = 0;
        public double SecondsPerFrame { get; set; }
        List<PhysicsFrame> frames;
        int lastIndex = -1;
        
        public AnimatedGameComponent(EasyTopDownGame game, string sheetName)
            : base(game)
        {
            base.imageName = sheetName;
            this.Category = sheetName;
            LayerDepth = LayerDepths.Middle;
            this.SecondsPerFrame = .2;
            spriteSheet = game.Content.Load<SpriteSheet>(sheetName);
            Initialize();
            InitializePhysics();
            Scale = 1;
        }

        public override void InitializePhysics()
        {
            Body = new Body(game.Physics);
            Body.UserData = this;
            Body.BodyType = BodyType.Dynamic;
            Body.FixedRotation = true;
            frames = new List<PhysicsFrame>();

            PhysicsFrame frame = PhysicsHelper.GetVerticesForTexture(spriteSheet, 0);
            List<Fixture> fixtures = FixtureFactory.AttachCompoundPolygon(frame.Vertices, 1f, Body);
            frame.Fixtures = fixtures;
            frames.Add(frame);
            PhysicsHelper.AttachOnCollisionHandlers(Body, game);
        }

        String BuildActiveSpriteName()
        {
            return ImageName + "-" + BuildActiveSpriteSuffix();
        }

        protected virtual String BuildActiveSpriteSuffix()
        {
            return currentAnimationFrame.ToString();
        }

        int GetCurrentSpriteSheetIndex()
        {
            string spriteName = BuildActiveSpriteName();
            int index = spriteSheet.GetIndex(spriteName);
            return index;
        }

        public override void Draw(GameTime gameTime)
        {            
            PhysicsFrame currentFrame = getCurrentPhysicsFrame();
            Rectangle sourceRectangle = GetCurrentFrameRectangle();
            this.game.SpriteBatch.Draw(spriteSheet.Texture, DisplayPosition, sourceRectangle, OverlayColor, Body.Rotation, currentFrame.Offset, Scale, SpriteEffects.None, LayerDepth);
        }

        protected int GetCurrentFrameIndex()
        {
            string spriteName = BuildActiveSpriteName();
            int index = spriteSheet.GetIndex(spriteName);
            return index;
        }

        protected Rectangle GetCurrentFrameRectangle()
        {
            int index = GetCurrentFrameIndex();
            return spriteSheet.SourceRectangle(index);
        }

        protected PhysicsFrame getCurrentPhysicsFrame()
        {
            int index = GetCurrentFrameIndex();
            PhysicsFrame currentFrame = frames[0];
            return currentFrame;
        }

        /// <summary>
        /// Updates the animation frame.  It is up to sub-class implementations to call this method.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        protected virtual int ClickAnimationFrame(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - lastFrameChange > SecondsPerFrame)
            {
                lastFrameChange = gameTime.TotalGameTime.TotalSeconds;
                if (currentAnimationFrame + 1 < framesPerDirection)
                {
                    currentAnimationFrame++;
                }
                else
                {
                    currentAnimationFrame = 0;
                }
            }           
            return currentAnimationFrame;
        }

        public override void Update(GameTime gameTime)
        {            
            base.Update(gameTime);            
            updateEnabledFixtures();            
        }

        private void updateEnabledFixtures()
        {
            //int index = GetCurrentSpriteSheetIndex();
            //if (index != lastIndex)
            //{
            //    if (lastIndex != -1)
            //    {
            //        frames[lastIndex].DisableFixtures();
            //    }
            //    frames[index].EnableFixtures();
            //    lastIndex = index;
            //}
        }

        public override int Width
        {
            get
            {
                Rectangle currentFrameRectangle = GetCurrentFrameRectangle();
                return currentFrameRectangle.Width;
            }
        }

        public override int Height
        {
            get
            {
                Rectangle currentFrameRectangle = GetCurrentFrameRectangle();
                return currentFrameRectangle.Height;
            }
        }
    }

    public enum AnimatedGameComponentDirection
    {
        Front,
        Back,
        Left,
        Right
    }

    
}
