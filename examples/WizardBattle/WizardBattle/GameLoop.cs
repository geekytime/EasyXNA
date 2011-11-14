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
        FourDirectionPlayerComponent wizard1;
        FourDirectionPlayerComponent wizard2;

        PlayerScoreDisplay display1;
        PlayerScoreDisplay display2;

        Rectangle viewableArea;        

        public override void Setup()
        {            
            viewableArea = AddWalls(16, 48, 27,43, "brick-0");

            

            wizard1 = AddFourDirectionPlayer(PlayerIndex.One, "wizard");
            wizard1.SetPosition(100, 300);

            display1 = AddPlayerScoreDisplay(PlayerIndex.One, "segoe");
            display1.SetPosition(20, 15);

            wizard2 = AddFourDirectionPlayer(PlayerIndex.Two, "wizard");
            wizard2.SetPosition(600, 300);

            display2 = AddPlayerScoreDisplay(PlayerIndex.Two, "segoe");
            display2.SetPosition(420, 15);

            wizard2.OverlayColor = Color.Orange;

            AddMonsters("blob", 1);
            AddMonsters("ghost", 1);
            AddMonsters("ogre", 1);            
     
            AddRuby();
 
            AddCollisionHandler("wizard", "ruby", WizardRubyCollision);
            this.AddBackgroundImage("tile", viewableArea);
        }        

        public void WizardRubyCollision(EasyGameComponent wizard, EasyGameComponent ruby)
        {
            EffectGameComponent effect = AddEffect("zap", ruby.DisplayPosition);
            display1.Score++;
            ruby.Remove();            
            AddRuby();

        }

        public void WizardMonsterCollision(EasyGameComponent wizard, EasyGameComponent monster)
        {
            wizard.Remove();
            
            AddEffect("colorexplosion", wizard.DisplayPosition);
            TextEffect textEffect = AddTextEffect("test", "U R D3@D!!!", wizard.DisplayPosition, Color.Red);
            textEffect.SecondsToLive = 1;
            textEffect.MakeFlashingText(Color.WhiteSmoke, .05);
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
                EasyGameComponent monster = this.AddRotatingWanderingComponent(imageName);
                monster.SetPosition(200,200);
            }
            this.AddCollisionHandler("wizard", imageName, WizardMonsterCollision);
            this.AddCollisionHandler("wizard", imageName, WizardMonsterCollision);
        }
    }
}

