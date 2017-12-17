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
    public class TriangleBullets : Attack
    {
        #region Triangle
        private TimeSpan triangleShotting = TimeSpan.Zero;
        private TimeSpan triangleShotting_pause = TimeSpan.Zero;

        private void triangleBullet(GameEngineBehaviors b)
        {
            triangleShotting += GameEngine.gameTime.ElapsedGameTime;
            if (triangleShotting > TimeSpan.FromSeconds(1))
            {
                triangleShotting -= TimeSpan.FromSeconds(1);
                double turn_x = b.Position.X;
                double turn_y = b.Position.Y;
                Vector2 shotting_point = new Vector2((float)turn_x, (float)turn_y);

                //bullet move #1
                double f_x = 0;
                double f_y = 0;
                double target_x = GameEngine.Player.Position.X;
                double target_y = GameEngine.Player.Position.Y;
                double shot_x = turn_x;
                double shot_y = turn_y;
                f_x = (target_x - shot_x) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                f_y = (target_y - shot_y) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));

                //bullet #1
                new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(.5f), new Vector2((float)f_x, (float)f_y), shotting_point);

                target_x = GameEngine.Player.Position.X-200;
                target_y = GameEngine.Player.Position.Y-200;
                shot_x = turn_x;
                shot_y = turn_y;
                f_x = (target_x - shot_x) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                f_y = (target_y - shot_y) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                
                //bullet #2
                new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(.5f), new Vector2((float)f_x, (float)f_y), shotting_point);

                target_x = GameEngine.Player.Position.X + 200;
                target_y = GameEngine.Player.Position.Y + 200;
                shot_x = turn_x;
                shot_y = turn_y;
                f_x = (target_x - shot_x) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                f_y = (target_y - shot_y) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));

                //bullet #3
                new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(.5f), new Vector2((float)f_x, (float)f_y), shotting_point);
            }
        }

        private void triangleBullet2(GameEngineBehaviors b)//backup, for emergency
        {
            triangleShotting_pause += GameEngine.gameTime.ElapsedGameTime;

            string triangleShotting_pause_string = triangleShotting_pause.TotalSeconds.ToString();
            double triangleShotting_pause_double = 0;
            double.TryParse(triangleShotting_pause_string, out triangleShotting_pause_double);

            if (triangleShotting_pause < TimeSpan.FromSeconds(1))
            {
                triangleShotting += GameEngine.gameTime.ElapsedGameTime;
                if (triangleShotting > TimeSpan.FromSeconds(0.2))
                {
                    triangleShotting -= TimeSpan.FromSeconds(0.2);
                    double turn_x = b.Position.X;
                    double turn_y = b.Position.Y;
                    Vector2 shotting_point = new Vector2((float)turn_x, (float)turn_y);

                    //bullet move #1
                    double f_x = 0;
                    double f_y = 0;
                    double target_x = GameEngine.Player.Position.X + triangleShotting_pause_double*4000;
                    double target_y = GameEngine.Player.Position.Y + triangleShotting_pause_double*4000;
                    double shot_x = turn_x;
                    double shot_y = turn_y;
                    f_x = (target_x - shot_x) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                    f_y = (target_y - shot_y) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));

                    //bullet #1
                    new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(.5f), new Vector2((float)f_x, (float)f_y), shotting_point);

                }
            }
            if (triangleShotting_pause > TimeSpan.FromSeconds(4))
            {
                triangleShotting_pause = TimeSpan.Zero;
            }
        }
        #endregion
        public override void Shoot(GameEngineBehaviors b)
        {
            this.triangleBullet(b);
        }
    }
}
