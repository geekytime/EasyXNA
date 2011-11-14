using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Factories;
using FarseerPhysics;
using FarseerPhysics.Collision;

namespace EasyXNA
{
    public class EasyGameComponent : DrawableGameComponent
    {
        //TODO: Hide the body, expose the concepts
        public Body Body { get; set; }
        Vector2 offset;

        protected InputHandler InputHandler { get; set; }        
        public float Scale { get; set; }
        public float LayerDepth { get; set; }

        protected EasyTopDownGame game;
        protected String imageName;

        /// <summary>
        /// Specifies what "kind" of component this is.  This value is used to handle collisions between object Categories.
        /// </summary>
        public String Category { get; set; }

        public String ImageName { get { return imageName; } }

        /// <summary>
        /// Sets whether this Component will move when it collides with other components
        /// 
        /// Default value is True
        /// </summary>
        public bool Static { get { return Body.IsStatic; } set { Body.IsStatic = value; } }

        public bool AllowVerticalMovement { get; set; }
        public bool AllowHorizontalMovement { get; set; }

        /// <summary>
        /// The 'tint' color used to draw this component.  Default value is Color.White.
        /// </summary>
        public Color OverlayColor { get; set; }

        /// <summary>
        /// Changes the position of the physics object in the simulator.  Don't mess with this unless you know what you're doing. :)
        /// </summary>
        public Vector2 SimPosition
        {
            get
            {
                return Body.Position;
            }
            set
            {
                Body.Position = value;
            }
        }

        /// <summary>
        /// Called DisplayPosition to differentiate it from SimPosition.
        /// 
        /// This value is in Pixels.
        /// 
        /// SetPosition can also be used to set the X,Y values directly.
        /// </summary>
        public Vector2 DisplayPosition
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(SimPosition);
            }
            set
            {
                SimPosition = ConvertUnits.ToSimUnits(value);
            }
        }

        /// <summary>
        /// Sets a random position for this EasyGameComponent, but only within the specified allowedRegion rectangle
        /// </summary>
        /// <param name="allowedRegion"></param>
        public void SetRandomPosition(Rectangle allowedRegion)
        {
            Vector2 newPosition;
            bool collision = false;
            do
            {
                newPosition = GenerateRandomDisplayPosition(allowedRegion);
                Func<Fixture, bool> callback = delegate(Fixture fixture)
                {
                    collision = true;
                    return false;
                };

                AABB newBox = new AABB(newPosition, Width, Height);

                game.Physics.QueryAABB(callback, ref newBox);

            } while (collision == true);


            this.DisplayPosition = newPosition;
        }

        private Vector2 GenerateRandomDisplayPosition(Rectangle allowedRegion)
        {
            int left = allowedRegion.Left + (Width / 2);
            int right = allowedRegion.Right - (Width / 2);
            int x = RandomHelper.IntInRange(left, right);

            int top = allowedRegion.Top + (Height / 2);
            int bottom = allowedRegion.Bottom - (Height / 2);
            int y = RandomHelper.IntInRange(top, bottom);
            return new Vector2(x, y);
        }


        Texture2D texture;

        /// <summary>
        /// Should represent how fast a character speeds up.  Unfortunately, right now it simply means 'Speed'.
        /// </summary>        
        public float Acceleration { get; set; }
        public float MaxVelocity { get; set; }


        /// <summary>
        /// Creates a new EasyGameComponent
        /// </summary>
        /// <param name="game">The EasyTopDownGame instance that this component will be attached to.</param>
        /// <param name="imageName">Sets ImageName()</param>
        public EasyGameComponent(EasyTopDownGame game, String imageName)
            : base(game)
        {
            this.game = game;
            this.imageName = imageName;
            this.Category = imageName;
            this.texture = this.game.Content.Load<Texture2D>(imageName);            
            initialize();
            InitializePhysics();
        }

        /// <summary>
        /// Can be called by implementers who do not want default values for imageName, Category, Texture or Physics
        /// </summary>
        /// <param name="game"></param>
        public EasyGameComponent(EasyTopDownGame game)
            : base(game)
        {
            this.game = game;
            
            initialize();
        }

        private void initialize()
        {
            this.AllowHorizontalMovement = true;
            this.AllowVerticalMovement = true;            
            this.OverlayColor = Color.White;
            this.Props = new CustomProperties();
            LayerDepth = LayerDepths.Middle;
            Scale = 1;
        }

        /// <summary>
        /// Converts image data to physics fixtures, and initializes default values 
        /// </summary>
        public virtual void InitializePhysics()
        {
            Body = new Body(game.Physics);
            Body.UserData = this;

            PhysicsFrame easyVertices = PhysicsHelper.GetVerticesForTexture(texture);
            FixtureFactory.AttachCompoundPolygon(easyVertices.Vertices, 1f, Body);
            offset = easyVertices.Offset;

            PhysicsHelper.AttachOnCollisionHandlers(Body, game);
        }

        /// <summary>
        /// Grabs the SpriteBatch for this game, and draws this components sprite to the screen.
        /// 
        /// This method is overridden by most of the animated GameComponent classes.
        /// </summary>
        /// <param name="gameTime">Current GameTime.  Needed by XNA.</param>
        public override void Draw(GameTime gameTime)
        {                       
            game.SpriteBatch.Draw(texture, DisplayPosition, null, OverlayColor, Body.Rotation, offset, Scale, SpriteEffects.None, LayerDepth);            
        }



        /// <summary>
        /// Sets the display position of this component in pixels.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(int x, int y)
        {
            this.DisplayPosition = new Vector2(x, y);
        }

        /// <summary>
        /// Returns the width of the image used by this component
        /// </summary>
        public virtual int Width { get { return texture.Width; } }

        /// <summary>
        /// Returns the height of the image used by this component
        /// </summary>
        public virtual int Height { get { return texture.Height; } }

        /// <summary>
        /// Removes this component from the game and physics engine.
        /// 
        /// TODO: Figure out a way to just disable Physics?  Remove & Replace?
        /// </summary>
        public void Remove()
        {
            if (IsRemoved == false)
            {
                this.game.RemoveComponent(this);
                this.Body.CollidesWith = FarseerPhysics.Dynamics.Category.None;
                this.game.Physics.RemoveBody(this.Body);
                IsRemoved = true;
            }
        }

        public bool Enabled
        {
            get
            {
                return !IsRemoved;
            }
            set
            {
                if (value == false)
                {
                    Remove();
                }
                else
                {
                    if (IsRemoved == true)
                    {
                        game.Components.Add(this);
                        game.Physics.BodyList.Add(this.Body);
                        IsRemoved = false;
                    }
                }
            }
        }


        /// <summary>
        /// Had a few issues with Remove() being called multiple times per frame, which was goofing up the Physics.  This is a hack to avoid that problem. :(        
        /// </summary>
        public bool IsRemoved { get; set; }

        /// <summary>
        /// Optional custom properties that can be used to store information about this component, and optionally rendered as part of this components DisplayData instance.
        /// </summary>
        public CustomProperties Props { get; set; }
    }
}
