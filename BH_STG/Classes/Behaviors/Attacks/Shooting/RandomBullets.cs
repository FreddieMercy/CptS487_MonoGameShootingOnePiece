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
    public class RandomBullets : Attack
    {
        #region Random

        private TimeSpan randomShotting = TimeSpan.Zero;
        private TimeSpan randomShotting_pause = TimeSpan.Zero;
        private void randomBullet(GameEngineBehaviors b)
        {
            //random shotting
            Random rd = new Random();
            randomShotting_pause += GameEngine.gameTime.ElapsedGameTime;
            string randomShotting_pause_string = randomShotting_pause.TotalSeconds.ToString();
            double randomShotting_pause_double = 0;
            double.TryParse(randomShotting_pause_string, out randomShotting_pause_double);

            randomShotting += GameEngine.gameTime.ElapsedGameTime;
            if (randomShotting_pause < TimeSpan.FromSeconds(3))
            {
                if (randomShotting > TimeSpan.FromSeconds(0.01))
                {
                    randomShotting -= TimeSpan.FromSeconds(0.01);
                    //shot point
                    double angle = Math.PI * 3 * randomShotting_pause_double;
                    double turn_x = b.Position.X + 25 * Math.Sin(angle) * randomShotting_pause_double;
                    double turn_y = b.Position.Y - 25 * Math.Cos(angle) * randomShotting_pause_double;
                    Vector2 shotting_point = new Vector2((float)turn_x, (float)turn_y);

                    //bullet move
                    double rd_num_x = rd.Next(1, GameEngine.graphic.PreferredBackBufferWidth);
                    double rd_num_y = rd.Next(1, GameEngine.graphic.PreferredBackBufferHeight);
                    double f_x = 0;
                    double f_y = 0;
                    double target_x = rd_num_x;
                    double target_y = rd_num_y;
                    double shot_x = b.Position.X;
                    double shot_y = b.Position.Y;
                    f_x = (target_x - shot_x) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                    f_y = (target_y - shot_y) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                    new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(.005f), new Vector2((float)f_x, (float)f_y), shotting_point);
                }
            }

            if (randomShotting_pause > TimeSpan.FromSeconds(8))
            {
                randomShotting_pause = TimeSpan.Zero;
            }
        }
        #endregion
        public override void Shoot(GameEngineBehaviors b)
        {
            this.randomBullet(b);
        }
    }
}
