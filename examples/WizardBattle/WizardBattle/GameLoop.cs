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

namespace WizardBattle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameLoop : EasyTopDownGame
    {
        EasyGameComponent ruby;
        AnimatedPlayer4DirectionGameComponent wizard1;
        AnimatedPlayer4DirectionGameComponent wizard2;

        Rectangle viewableArea;        

        public override void Setup()
        {            
            viewableArea = AddWalls(16, 48, 27,43, "brick-0");
            this.AddBackgroundImage("tile", viewableArea);

            wizard1 = AddAnimatedAdventurePlayer(PlayerIndex.One, "wizard");
            wizard1.SetPosition(100, 300);
            wizard1.DisplayData.SetPosition(35, 10);

            wizard2 = AddAnimatedAdventurePlayer(PlayerIndex.Two, "wizard");
            wizard2.DisplayData.SetPosition(1000, 10);
            wizard2.SetPosition(600, 300);

            wizard2.OverlayColor = Color.Orange;

            AddMonsters("blob", 1);
            AddMonsters("ghost", 1);
            AddMonsters("ogre", 1);            
     
            AddRuby();
 
            AddCollisionHandler("wizard", "ruby", WizardRubyCollision);                                    
        }        

        public void WizardRubyCollision(EasyGameComponent wizard, EasyGameComponent ruby)
        {
            ((AnimatedPlayer4DirectionGameComponent)wizard).PlayerDisplayData.Score++;
            ruby.Remove();
            AddRuby();            
        }

        public void WizardMonsterCollision(EasyGameComponent wizard, EasyGameComponent monster)
        {
            wizard.Remove();
        }

        public void AddRuby()
        {
            ruby = this.AddComponent("ruby");
            ruby.SetRandomPosition(viewableArea);
        }

        public void AddMonsters(string imageName, int count)
        {
            for (int i = 0; i < count; i++)
            {
                EasyGameComponent monster = this.AddWanderingEnemy(imageName);
                monster.SetRandomPosition(viewableArea);
            }
            this.AddCollisionHandler("wizard", imageName, WizardMonsterCollision);
            this.AddCollisionHandler("wizard", imageName, WizardMonsterCollision);
        }
    }
}

