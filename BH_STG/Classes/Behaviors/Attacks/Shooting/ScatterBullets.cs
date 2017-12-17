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
    public class ScatterBullets : Attack
    {
        #region Scatter
        private TimeSpan scatterShotting = TimeSpan.Zero;
        private void scatterBullet(GameEngineBehaviors b)
        {
            scatterShotting += GameEngine.gameTime.ElapsedGameTime;
            if (scatterShotting > TimeSpan.FromSeconds(2))
            {
                scatterShotting -= TimeSpan.FromSeconds(2);
                double angle = 2 * Math.PI / 24;
                double f_x = 0;
                double f_y = 0;
                for (int i = 1; i <= 2 * Math.PI / angle; i++)
                {
                    f_x = Math.Sin(angle * i);
                    f_y = Math.Cos(angle * i);
                    new Bullet(b, Images.Scattershot_bullet, DefaultSizes.DefaultBulletSize, new EnemyBulletBehavior(), new Vector2((float)f_x, (float)f_y));
                }
            }
        }
        #endregion
        public override void Shoot(GameEngineBehaviors b)
        {
            this.scatterBullet(b);
        }
    }
}
