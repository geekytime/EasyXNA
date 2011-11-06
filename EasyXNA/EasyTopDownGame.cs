﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics;
using FarseerPhysics.Dynamics.Contacts;

namespace EasyXNA
{
    /// <summary>
    /// A class to represent a simple "top-down" game.  Physics elements will emulate top-down "pool-table-style" physics.
    /// </summary>
    public abstract class EasyTopDownGame : Game
    {
        /// <summary>
        /// Should be overridden by implementers.  This will be called during LoadContent, so it is safe to call content-related methods from Setup().
        /// </summary>
        public abstract void Setup();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;
        int gridSize = 16;

        const int defaultScreenWidth = 704;
        const int defaultScreenHeight = 480;

        int screenWidth;
        int screenHeight;

        /// <summary>
        /// Gets the SpriteBatch used to draw images
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return this.spriteBatch; }
        }

        /// <summary>
        /// The CollisionManager keeps track of which physics collisions we want to attempt to handle.
        /// </summary>
        public CollisionManager CollisionManager { get; set; }

        /// <summary>
        /// Sets the default font that should be used to render player data.
        /// 
        /// Note: This method is subject to change
        /// </summary>
        public string PlayerFontName { get; set; }

        /// <summary>
        /// Creates an EasyTopDownGameObject
        /// </summary>
        public EasyTopDownGame()
        {
            this.TargetElapsedTime = TimeSpan.FromSeconds(1 / 60f);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            world = new World(Vector2.Zero);
            CollisionManager = new CollisionManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            screenWidth = defaultScreenWidth;
            screenHeight = defaultScreenHeight;

            base.Initialize();
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
        }

        public int RowCount { get { return screenHeight / gridSize; } }

        public int ColCount { get { return screenWidth / gridSize; } }

        public World Physics { get { return world; } }

        public int ScreenWidth { get { return screenWidth; } }
        public int ScreenHeight { get { return screenHeight; } }

        public Vector2 GridToVector(int col, int row, int width, int height)
        {
            int x = col * gridSize + width / 2;
            int y = row * gridSize + height / 2;
            return new Vector2(x, y);
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
            Setup();
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
            world.Step((float)this.TargetElapsedTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Adds a simple user-controller player to the game
        /// </summary>
        /// <param name="playerIndex">The GamePad number to tie this player object to.  Keyboard keys will also be mapped based on this index.</param>
        /// <param name="imageName">The image to use for the player</param>
        /// <returns>A PlayerGameComponent</returns>
        public PlayerGameComponent AddPlayer(PlayerIndex playerIndex, String imageName)
        {
            PlayerGameComponent playerGameComponent = new PlayerGameComponent(this, imageName, playerIndex);
            AddComponent(playerGameComponent);
            return playerGameComponent;
        }

        /// <summary>
        /// Adds a tiled background image that will be scaled to fit the given Rectangle area
        /// </summary>
        /// <param name="imageName">The image tile to use</param>
        /// <param name="tileArea">Useful in conjunction with AddWalls(), which returns a Rectangle</param>
        public void AddBackgroundImage(string imageName, Rectangle tileArea)
        {
            BackgroundGameComponent background = new BackgroundGameComponent(this, imageName, tileArea);
            Components.Add(background);
        }

        /// <summary>
        /// Adds the image specified in a box-shape, starting at the X and Y values, and building the specified number of rows and columns.
        /// </summary>
        /// <param name="startX">The X component of the upper-left hand corner of the box to draw, in pixels</param>
        /// <param name="startY">The Y component of the upper-left hand corner of the box to draw, in pixels</param>
        /// <param name="rows">The number of "bricks" to draw across the top and bottom of the box</param>
        /// <param name="columns">The number of "bricks" to draw across the left and right sides of the box</param>
        /// <param name="imageName">The image to draw</param>
        /// <returns>A Rectangle that represents the "inner bounds" of the box created</returns>
        public Rectangle AddWalls(int startX, int startY, int rows, int columns, string imageName)
        {
            EasyGameComponent firstWall = null;
            EasyGameComponent lastWall = null;
            for (int row = 0; row < rows; row++)
            {
                if (row == 0 || row == rows - 1)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        EasyGameComponent wall = AddWall(startX, startY, row, col, imageName);
                        if (row == 0 && col == 0)
                        {
                            firstWall = wall;
                        }
                        if (row == rows - 1 && col == columns - 1)
                        {
                            lastWall = wall;
                        }
                    }
                }
                else
                {
                    for (int col = 0; col < columns; col = col + columns - 1)
                    {
                        AddWall(startX, startY, row, col, imageName);

                    }
                }
            }
            Rectangle viewableArea = new Rectangle();
            viewableArea.X = (int)firstWall.DisplayPosition.X + (firstWall.Width / 2);
            viewableArea.Y = (int)firstWall.DisplayPosition.Y + (firstWall.Height / 2);
            viewableArea.Width = (int)lastWall.DisplayPosition.X - (lastWall.Width / 2) - viewableArea.X;
            viewableArea.Height = (int)lastWall.DisplayPosition.Y - (lastWall.Height / 2) - viewableArea.Y;
            return viewableArea;
        }

        private EasyGameComponent AddWall(int startX, int startY, int row, int col, string imageName)
        {
            EasyGameComponent wall = this.AddComponent(imageName);
            int x = (startX) + (col * wall.Width);
            int y = (startY) + (row * wall.Height);
            wall.Static = true;
            wall.SetPosition(x, y);
            return wall;
        }

        /// <summary>
        /// Adds an animated player to the game.
        /// </summary>
        /// <param name="playerIndex">The player index to check for input, used for both GamePad and Keyboard input</param>
        /// <param name="imageName">This should point to an XML sprite-sheet.  TODO:Link to SpriteSheet example</param>
        /// <returns>An AnimatedPlayer4DirectionGameComponent</returns>
        public AnimatedPlayer4DirectionGameComponent AddAnimatedAdventurePlayer(PlayerIndex playerIndex, String imageName)
        {
            AnimatedPlayer4DirectionGameComponent animatedAdventurePlayerGameComponent = new AnimatedPlayer4DirectionGameComponent(this, imageName, playerIndex);
            AddComponent(animatedAdventurePlayerGameComponent);
            return animatedAdventurePlayerGameComponent;
        }

        /// <summary>
        /// Adds an animated player to the game.
        /// </summary>
        /// <param name="playerIndex">The player index to check for input, used for both GamePad and Keyboard input</param>
        /// <param name="imageName">This should point to an XML sprite-sheet.  TODO:Link to SpriteSheet example</param>
        /// <param name="category">Use this to add a second player with different collision rules.</param>
        /// <returns></returns>
        public AnimatedPlayer4DirectionGameComponent AddAnimatedAdventurePlayer(PlayerIndex playerIndex, String imageName, String category)
        {
            AnimatedPlayer4DirectionGameComponent animatedAdventurePlayerGameComponent = new AnimatedPlayer4DirectionGameComponent(this, imageName, playerIndex);
            animatedAdventurePlayerGameComponent.Category = category;
            AddComponent(animatedAdventurePlayerGameComponent);
            return animatedAdventurePlayerGameComponent;
        }

        /// <summary>
        /// Removes the specified Component from the game
        /// </summary>
        /// <param name="component">The component to Remove</param>
        public void RemoveComponent(EasyGameComponent component)
        {
            this.Components.Remove(component);
        }

        /// <summary>
        /// Adds a game component that will wander arounda aimlessly until it bumps into something.  Then it turns around and wanders some more.
        /// </summary>
        /// <param name="imageName">The image to use for the wandering enemy.  This should be a an XML Sprite sheet</param>
        /// <returns>An animated rotating enemy component.</returns>
        public AnimatedRotatingEnemyComponent AddWanderingEnemy(String imageName)
        {
            AnimatedRotatingEnemyComponent wanderingEnemyGameComponent = new AnimatedRotatingEnemyComponent(this, imageName);
            AddComponent(wanderingEnemyGameComponent);
            return wanderingEnemyGameComponent;
        }

        /// <summary>
        /// This is a callback for the physics engine so that it can raise collision events back to the game
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public bool OnFixtureCollision(Fixture f1, Fixture f2, Contact contact)
        {
            EasyGameComponent componentOne = (EasyGameComponent)f1.Body.UserData;
            EasyGameComponent componentTwo = (EasyGameComponent)f2.Body.UserData;
            bool collisionHandled = CollisionManager.FireEvent(componentOne, componentTwo);
            return true;
        }


        /// <summary>
        /// Implementers should call this method to register a collision handler function with the physics engine.
        /// Whenever two objects of the given categories collide, the handlerFunction will be invoked.
        /// </summary>
        /// <param name="categoryOne">The first kind of object to check</param>
        /// <param name="categoryTwo">The second kind of obejct to check</param>
        /// <param name="handlerFunction">The function to call whenever two objects of the given kind collide</param>
        public void AddCollisionHandler(String categoryOne, String categoryTwo, Action<EasyGameComponent, EasyGameComponent> handlerFunction)
        {
            CollisionManager.AddHandler(categoryOne, categoryTwo, handlerFunction);
        }

        /// <summary>
        /// Adds a simple component to the game using the given image.
        /// </summary>
        /// <param name="imageName">The name of the image to load and draw for this component</param>
        /// <returns>A simple EasyGameComponent object</returns>
        public EasyGameComponent AddComponent(string imageName)
        {
            EasyGameComponent easyGameComponent = new EasyGameComponent(this, imageName);
            AddComponent(easyGameComponent);
            return easyGameComponent;
        }

        /// <summary>
        /// Allows implementers to pass in their own extensions of EasyGameComponent
        /// </summary>
        /// <param name="component"></param>
        protected void AddComponent(EasyGameComponent component)
        {
            this.Components.Add(component);
        }
    }
}