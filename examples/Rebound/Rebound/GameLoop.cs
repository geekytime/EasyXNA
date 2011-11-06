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
            viewableArea = AddWalls(35, 65, 40, 35, "brick-1x1");
            //this.AddBackgroundImage("tile", viewableArea);

            ball = AddComponent("ball");            
            ball.Body.Friction = 0;
            ball.Body.FixedRotation = true;
            ball.Body.Restitution = 1;
            AddPaddle();            
        }

        public EasyGameComponent AddPaddle()
        {
            //TODO: Create a slightly more wedge-shaped paddle
            paddle = this.AddPlayer(PlayerIndex.One, "paddle");
            paddle.AllowVerticalMovement = false;
            paddle.SetPosition(200, 500);         
            return paddle;
        }


    }
}

