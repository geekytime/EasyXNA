using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public class ProjectileComponent : AnimatedGameComponent
    {        
        public ProjectileComponent(EasyTopDownGame game, EasyGameComponent originatingComponent, string sheetName, Vector2 direction, float velocity)
            : base(game, sheetName)
        {                    
            SetStartPosition(originatingComponent, direction);
            SetInMotion(direction, velocity);
            this.Body.IsBullet = true;
        }

        private void SetStartPosition(EasyGameComponent originatingComponent, Vector2 direction)
        {
            Vector2 positionOffset = Vector2.Zero;
            if (direction.Equals(ProjectileDirections.Up))
            {
                positionOffset = originatingComponent.Height * .6f * direction;                
            }
            else if (direction.Equals(ProjectileDirections.Down))
            {
                positionOffset = originatingComponent.Height * .6f * direction;                
            }
            else if (direction.Equals(ProjectileDirections.Left))
            {
                positionOffset = originatingComponent.Width * .6f * direction;                
            }
            else if (direction.Equals(ProjectileDirections.Right))
            {
                positionOffset = originatingComponent.Width * .6f * direction;                
            }
            DisplayPosition = originatingComponent.DisplayPosition + positionOffset;
            
        }

        private void SetInMotion(Vector2 direction, float velocity)
        {
            Vector2 actualVelocity = direction * velocity;
            this.Body.ApplyLinearImpulse(actualVelocity);
        }

        public override void Update(GameTime gameTime)
        {
            ClickAnimationFrame(gameTime);
            base.Update(gameTime);
        }

    }
    
    public static class ProjectileDirections
    {
        public static Vector2 Up = new Vector2(0, -1);
        public static Vector2 Down = new Vector2(0, 1);
        public static Vector2 Left = new Vector2(-1, 0);
        public static Vector2 Right = new Vector2(1, 0);
    }
}
