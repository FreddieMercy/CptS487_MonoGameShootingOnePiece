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
    public class ScatterBulletsTwo : Attack
    {
        private TimeSpan scatterShooting = TimeSpan.Zero;
        private void ScatterTwoBullets(GameEngineBehaviors b)
        {
            double angle = 2 * Math.PI / 24;
            double f_x = 0;
            double f_y = 0;
            for (int i = 1; i <= 2 * Math.PI / angle; i++)
            {
                f_x = Math.Sin(angle * i);
                f_y = Math.Cos(angle * i);
                new BulletScatterTwo(b, new Vector2((float)f_x, (float)f_y));
            }
        }
        public override void Shoot(GameEngineBehaviors b)
        {
            scatterShooting += GameEngine.gameTime.ElapsedGameTime;
            if (scatterShooting > TimeSpan.FromSeconds(4))
            {
                scatterShooting -= TimeSpan.FromSeconds(4);
                ScatterTwoBullets(b);
            }
        }
    }
}
