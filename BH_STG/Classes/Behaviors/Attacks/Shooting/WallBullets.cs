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
    public class WallBullets : Attack
    {
        #region Wall
        private TimeSpan wallBulletShotting = TimeSpan.Zero;
        private TimeSpan wallBulletShotting_pause = TimeSpan.Zero;
        private void wallBullet(GameEngineBehaviors b)
        {
            //random shotting
            Random rd = new Random();
            wallBulletShotting_pause += GameEngine.gameTime.ElapsedGameTime;
            string wallBulletShotting_pause_string = wallBulletShotting_pause.TotalSeconds.ToString();
            double wallBulletShotting_pause_double = 0;
            double.TryParse(wallBulletShotting_pause_string, out wallBulletShotting_pause_double);

            wallBulletShotting += GameEngine.gameTime.ElapsedGameTime;
            if (wallBulletShotting_pause < TimeSpan.FromSeconds(3))
            {
                if (wallBulletShotting < TimeSpan.FromSeconds(1))
                {
                    //shot point
                    double turn_x = b.Position.X + rd.Next(-25, 25);
                    double turn_y = b.Position.Y - rd.Next(-25, 25);
                    Vector2 shotting_point = new Vector2((float)turn_x, (float)turn_y);

                    //bullet move
                    double rd_num_x = rd.Next(1, GameEngine.graphic.PreferredBackBufferWidth);
                    double rd_num_y = rd.Next(1, GameEngine.graphic.PreferredBackBufferHeight / 2);
                    double f_x = 0;
                    double f_y = 0;
                    double target_x = rd_num_x;
                    double target_y = rd_num_y;
                    double shot_x = b.Position.X;
                    double shot_y = b.Position.Y;
                    f_x = (target_x - shot_x) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                    f_y = (target_y - shot_y) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                    // new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(), new Vector2((float)f_x, (float)f_y), shotting_point);
                    
                    new Bullet(b, Images.Scattershot_Bullet_wall, new Vector2(15, 25), new EnemyBulletBehavior(.005f),
                        new Vector2((float)1, (float)1), new Vector2((float)b.Position.X, (float)25 + b.Position.Y));
                    new Bullet(b, Images.Scattershot_Bullet_wall, new Vector2(15, 25), new EnemyBulletBehavior(.005f),
                         new Vector2((float).5, (float).5), new Vector2((float)-15 + b.Position.X, (float)0 + b.Position.Y));
                    new Bullet(b, Images.Scattershot_Bullet_wall, new Vector2(15, 25), new EnemyBulletBehavior(.005f),
                         new Vector2((float)1, (float)-1), new Vector2((float)2 + b.Position.X, (float)12 + b.Position.Y));
                    new Bullet(b, Images.Scattershot_Bullet_wall, new Vector2(15, 25), new EnemyBulletBehavior(.005f),
                         new Vector2((float)-1, (float)1), new Vector2((float)15 + b.Position.X, (float)-6 + b.Position.Y));
                    new Bullet(b, Images.Scattershot_Bullet_wall, new Vector2(15, 25), new EnemyBulletBehavior(.005f),
                         new Vector2((float)-1, (float)-1), new Vector2((float)-9 + b.Position.X, (float)33 + b.Position.Y));
                }
            }

            if (wallBulletShotting_pause > TimeSpan.FromSeconds(8))
            {
                wallBulletShotting_pause = TimeSpan.Zero;
                wallBulletShotting = TimeSpan.Zero;
            }
        }
        #endregion

        public override void Shoot(GameEngineBehaviors b)
        {
            this.wallBullet(b);
        }
    }
}