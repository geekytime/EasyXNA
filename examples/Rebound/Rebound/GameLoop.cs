using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EasyXNA;

namespace Rebound
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameLoop : EasyTopDownGame
    {
        private PlayerGameComponent paddle;
        EasyGameComponent ball;
        Rectangle viewableArea;

        public override void Setup()
        {
            viewableArea = AddWalls( "brick-1x1");
         
            this.AddBackgroundImage("background", viewableArea);
            AddPaddle();
            AddBall();
            AddBricks(4,30);
            AddCollisionHandler("ball", "brick", BallBrickCollision);
            AddInputHandler(PlayerNudge, PlayerIndex.One, Keys.Up);
        }

        public void PlayerNudge()
        {
            ball.Nudge(0, -.1f);
        }

        public void BallBrickCollision(EasyGameComponent ball, EasyGameComponent brick)
        {
            brick.Remove();
            EffectGameComponent effect = AddEffect("colorexplosion", brick.DisplayPosition);
            effect.SecondsPerFrame = .05;
            effect.OverlayColor = brick.OverlayColor;
        }

        public void AddBricks(int rows, int cols)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    AddBrick(r, c);
                }
            }
        }

        public EasyGameComponent AddBrick(int row, int col)
        {
            int start = 60;
            EasyGameComponent brick = AddComponent("brick-1x2");
            brick.Category = "brick";
            brick.SetPosition(start + (col * brick.Width),start + (row * brick.Height));
            brick.OverlayColor = RandomHelper.Color();
            return brick;
        }

        public EasyGameComponent AddBall()
        {
            ball = AddComponent("ball");
            ball.EdgeFriction = 0;
            ball.Rotateable = true;
            ball.BounceFactor = 1;
            ball.Static = false;
            ball.SetPosition(200, 700);
            ball.IsBullet = true;
            ball.Nudge(.6f, -.6f);
            ball.MaxVelocity = 2f;
            ball.ConstantVelocity = 10;
            return ball;
        }

        public EasyGameComponent AddPaddle()
        {
            paddle = this.AddPlayer(PlayerIndex.One, "paddle");
            paddle.AllowVerticalMovement = false;
            paddle.SetPosition(200, 775);
            paddle.Acceleration = 400;
            paddle.Mass = 50;
            paddle.BounceFactor = 1.1f;
            return paddle;
        }


    }
}

