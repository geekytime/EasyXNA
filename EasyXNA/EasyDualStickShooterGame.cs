using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EasyXNA
{
    public class EasyDualStickShooterGame : Game
    {        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public SpriteBatch SpriteBatch
        {
            get { return this.spriteBatch; }            
        }

        public EasyDualStickShooterGame()
        {     
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;            
            graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load other content AFTER we create the sprite batch...
            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }

        public PlayerGameComponent addPlayer(PlayerIndex playerIndex, String imageName)
        {
            PlayerGameComponent playerGameComponent = new PlayerGameComponent(this, imageName, playerIndex);
            this.Components.Add(playerGameComponent);            
            return playerGameComponent;
        }

        public WanderingEnemyGameComponent addWanderingEnemy(String imageName)
        {
            WanderingEnemyGameComponent wanderingEnemyGameComponent = new WanderingEnemyGameComponent(this, imageName);
            this.Components.Add(wanderingEnemyGameComponent);
            return wanderingEnemyGameComponent;

        }
    }
}