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
        int newMonsterCount = 0;

        public override int ScreenHeight { get { return 720; } }

        public override int ScreenWidth { get { return 1280; } }

        public override void Setup()
        {            
            viewableArea = AddWalls(16, 48, 27,43, "brick");

            AddWizard1();

            display1 = AddPlayerScoreDisplay(PlayerIndex.One, "segoe");
            display1.SetPosition(20, 15);

            wizard2 = AddFourDirectionPlayer(PlayerIndex.Two, "wizard");
            wizard2.Category = "wizard2";
            wizard2.SetPosition(600, 300);
            wizard2.OverlayColor = Color.Orange;

            display2 = AddPlayerScoreDisplay(PlayerIndex.Two, "segoe");
            display2.SetPosition(420, 15);

            AddMonsters("blob", 2);
            AddMonsters("ghost", 2);
            AddMonsters("ogre", 2);

            AddTimedEvent(3, AddRandomMonster);
     
            AddRuby();
 
            AddCollisionHandler("wizard", "ruby", WizardRubyCollision);
            AddBackgroundImage("tile", viewableArea);

            AddInputHandler(PlayerOneFireball, PlayerIndex.One, Keys.RightControl, Buttons.A);
            AddInputHandler(PlayerTwoFireball, PlayerIndex.Two, Keys.LeftControl, Buttons.A);

            //AddCollisionHandler("magicball", "brick", FireballBrickCollision);
            AddCollisionHandler("magicball", "monster", FireballMonsterCollision);
            AddCollisionHandler("wizard", "monster", WizardMonsterCollision);            
        }

        public void AddWizard1()
        {
            wizard1 = AddFourDirectionPlayer(PlayerIndex.One, "wizard");
            wizard1.SetPosition(100, 300);
        }

        public void AddRandomMonster()
        {
            if (newMonsterCount < 2)
            {
                newMonsterCount++;
                string monsterName = RandomHelper.PickOne("blob", "ghost", "ogre");
                AddMonsters(monsterName, newMonsterCount);
            }
        }

        public void PlayerOneFireball()
        {
            ProjectileComponent magicball = AddProjectile(wizard1, "magicball", 1);
            magicball.Body.Restitution = .8f;
        }

        public void PlayerTwoFireball()
        {
            if (wizard2.IsRemoved == false)
            {
                AddProjectile(wizard2, "magicball", 1);
            }
        }

        public void FireballBrickCollision(EasyGameComponent fireball, EasyGameComponent brick)
        {
            fireball.Remove();
        }

        public void FireballMonsterCollision(EasyGameComponent fireball, EasyGameComponent monster)
        {
            AddEffect("colorexplosion", monster.DisplayPosition);
            monster.Remove();
            fireball.Remove();
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
            TextEffect textEffect = AddTextEffect("segoe", "U R D3@D!!!", wizard.DisplayPosition, Color.Red);
            textEffect.SecondsToLive = 1;
            textEffect.MakeFlashingText(Color.WhiteSmoke, .05);
            AddTimedEvent(3, AddWizard1, 1);
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
                monster.Category = "monster";
                monster.SetRandomPosition(viewableArea);
            }
           
        }
    }
}

