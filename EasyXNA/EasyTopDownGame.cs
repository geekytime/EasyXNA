using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Input;

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

        public ScreenHelper ScreenHelper;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;
        int gridSize = 16;

        const int defaultScreenWidth =1600 ;
        const int defaultScreenHeight = 900;

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
            this.ScreenHelper = new ScreenHelper(this.GraphicsDevice.DisplayMode.TitleSafeArea);

            base.Initialize();

            graphics.PreferredBackBufferWidth = defaultScreenWidth;
            graphics.PreferredBackBufferHeight = defaultScreenHeight;
            graphics.ApplyChanges();
        }

        public World Physics { get { return world; } }

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

            spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            foreach (GameComponent component in Components)
            {
                if (component is DrawableGameComponent)
                {
                    ((DrawableGameComponent)component).Draw(gameTime);
                }
            }
            spriteBatch.End();
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

        public void AddTimedEvent(double interval, Action callback)
        {
            AddTimedEvent(interval, callback, -1);
        }

        public void AddTimedEvent(double interval, Action callback, int maxCount)
        {
            TimedEventComponent timedEvent = new TimedEventComponent(this, interval, callback, maxCount);
            Components.Add(timedEvent);
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

        public Rectangle AddWalls(params string[] imageNames)
        {
            return AddWalls(0, 0, imageNames);
        }

        public Rectangle AddWalls(int xPaddingLeft, int yPaddingTop, params string[] imageNames)
        {
            Rectangle tsa = this.GraphicsDevice.DisplayMode.TitleSafeArea;
            tsa.Width = tsa.Width - xPaddingLeft;
            tsa.Height = tsa.Height - yPaddingTop;
            tsa.X = tsa.X + xPaddingLeft;
            tsa.Y = tsa.Y + yPaddingTop;

            Texture2D wall = Content.Load<Texture2D>(imageNames[0]);
            int columns = tsa.Width / wall.Width;
            int rows = tsa.Height / wall.Height;
            int xOffset = (tsa.Width - (columns * wall.Width)) / 2;
            int yOffset = (tsa.Height - (rows * wall.Height)) / 2;
            int startX = tsa.Left + xOffset;
            int startY = tsa.Top + yOffset;
            return AddRectangle(startX, startY, rows, columns, imageNames);
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
        public Rectangle AddWalls(int startX, int startY, int rows, int columns, params string[] imageNames)
        {
            return AddRectangle(startX, startY, rows, columns, imageNames);
        }

        public Rectangle AddRectangle(int startX, int startY, int rows, int columns, params string[] imageNames)
        {
            EasyGameComponent firstWall = null;
            EasyGameComponent lastWall = null;
            for (int row = 0; row < rows; row++)
            {
                if (row == 0 || row == rows - 1)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        string imageName = RandomHelper.PickOne(imageNames);
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
                        string imageName = RandomHelper.PickOne(imageNames);
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
        public FourDirectionPlayerComponent AddFourDirectionPlayer(PlayerIndex playerIndex, String imageName)
        {
            FourDirectionPlayerComponent animatedAdventurePlayerGameComponent = new FourDirectionPlayerComponent(this, imageName, playerIndex);
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
        public FourDirectionPlayerComponent AddFourDirectionPlayer(PlayerIndex playerIndex, String imageName, String category)
        {
            FourDirectionPlayerComponent animatedAdventurePlayerGameComponent = new FourDirectionPlayerComponent(this, imageName, playerIndex);
            animatedAdventurePlayerGameComponent.Category = category;
            AddComponent(animatedAdventurePlayerGameComponent);
            return animatedAdventurePlayerGameComponent;
        }

        public EffectGameComponent AddEffect(String sheetName, Vector2 position)
        {
            EffectGameComponent effect = new EffectGameComponent(this, sheetName);
            effect.Position = position;
            Components.Add(effect);
            return effect;
        }

        public EffectGameComponent AddEffect(String sheetName, Vector2 position, double secondsPerFrame, int maxLoops)
        {
            EffectGameComponent effect = AddEffect(sheetName, position);
            effect.SecondsPerFrame = secondsPerFrame;
            effect.MaxLoops = maxLoops;
            return effect;
        }

        public void AddInputHandler(Action callback, PlayerIndex playerIndex, params object[] inputs)
        {
            InputHandlerComponent inputHandlerComponent = new InputHandlerComponent(this, callback, playerIndex, inputs);
            Components.Add(inputHandlerComponent);
        }

        public ProjectileComponent AddProjectile(FourDirectionPlayerComponent component, string sheetName, float acceleration)
        {
            Vector2 direction = component.GetProjectileDirection();
            ProjectileComponent projectile = new ProjectileComponent(this, component, sheetName, direction, acceleration);
            Components.Add(projectile);
            return projectile;
        }


        /// <summary>
        /// PlayerDisplayData provides a basic text for a player's number and score - "Player 1 - Score: 0"
        /// </summary>
        /// <param name="playerIndex">Which player is this data for?</param>
        /// <param name="fontName">Which font name to use.  You must have a corresponding spritefont in your content project.</param>
        /// <returns>A PlayerDisplayData object with the default configuration.</returns>
        public PlayerScoreDisplay AddPlayerScoreDisplay(PlayerIndex playerIndex, String fontName)
        {
            PlayerScoreDisplay playerScoreDisplay = new PlayerScoreDisplay(this, playerIndex, fontName);
            Components.Add(playerScoreDisplay);
            return playerScoreDisplay;
        }

        /// <summary>
        /// Adds a TextEffect to the screen with the given message.
        /// Make sure to set the position and seconds to live for text effects
        /// </summary>
        /// <param name="fontName">Which font name to use.  You must have a corresponding spritefont in your content project.</param>
        /// <param name="message">The message to display.</param>
        /// <returns>A PlayerDisplayData object with the default configuration.</returns>
        public TextEffect AddTextEffect(string fontName, string message)
        {
            TextEffect textEffect = new TextEffect(this, fontName, message);
            Components.Add(textEffect);
            return textEffect;
        }

        /// <summary>
        /// Adds a text effect to the screen with the given message, position, and color.
        /// </summary>
        /// <param name="fontName">Which font name to use.  You must have a corresponding spritefont in your content project.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="position">Where to display the message.</param>
        /// <param name="fontColor">The color to use when drawing the font.</param>
        /// <returns></returns>
        public TextEffect AddTextEffect(string fontName, string message, Vector2 position, Color fontColor)
        {
            TextEffect textEffect = new TextEffect(this, fontName, message);
            textEffect.Position = position;
            textEffect.FontColor = fontColor;
            Components.Add(textEffect);
            return textEffect;
        }


        /// <summary>
        /// Removes the specified Component from the game
        /// </summary>
        /// <param name="component">The component to Remove</param>
        public void RemoveComponent(DrawableGameComponent component)
        {
            this.Components.Remove(component);
        }

        /// <summary>
        /// Adds a game component that will wander arounda aimlessly until it bumps into something.  Then it turns around and wanders some more.
        /// </summary>
        /// <param name="imageName">The image to use for the wandering enemy.  This should be a an XML Sprite sheet</param>
        /// <returns>An animated rotating enemy component.</returns>
        public RotatingWanderingComponent AddRotatingWanderingComponent(String imageName)
        {
            RotatingWanderingComponent rotatingWanderingComponent = new RotatingWanderingComponent(this, imageName);
            AddComponent(rotatingWanderingComponent);
            return rotatingWanderingComponent;
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