using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace BH_STG
{
    public class ButterflyAttack : Attack
    {
        private Attack shooting = new TriangleBullets();
        private bool Summoned = false;
        public ButterflyAttack(Attack a = null)
        {
            if(a != null)
            {
                shooting = a;
            }
        }
        public override void Shoot(GameEngineBehaviors b)
        {
            shooting.Shoot(b);
            if(!Summoned && (b as Character).Lives == 1234)
            {
                //surprise
                Summoned = true;
                float h = (float) ((double)Images.MidBossCompanion.Height / Images.MidBossCompanion.Width) * GameEngine.graphic.PreferredBackBufferWidth;
                new Enemy(Images.MidBossCompanion, new Vector2(GameEngine.graphic.PreferredBackBufferWidth, h), new Vector2(0, -h + 1), new Vector2(0, 5), null, 1234);
            }
        }
    }
}
