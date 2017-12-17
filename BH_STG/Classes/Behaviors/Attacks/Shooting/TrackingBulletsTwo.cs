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
    public class TrackingBulletsTwo : Attack
    {
        private TimeSpan trackingShooting = TimeSpan.Zero;
        private TimeSpan trackingShooting_pause = TimeSpan.Zero;
        private void trackingBullet_mode2(GameEngineBehaviors b)
        {
            trackingShooting_pause += GameEngine.gameTime.ElapsedGameTime;
            string trackingShotting_pause_string = trackingShooting_pause.TotalSeconds.ToString();
            double trackingShotting_pause_double = 0;
            double.TryParse(trackingShotting_pause_string, out trackingShotting_pause_double);

            if (trackingShooting_pause < TimeSpan.FromSeconds(2))
            {
                trackingShooting += GameEngine.gameTime.ElapsedGameTime;
                if (trackingShooting > TimeSpan.FromSeconds(0.02))
                {
                    trackingShooting -= TimeSpan.FromSeconds(0.02);
                    //shot point #1
                    double angle = Math.PI * 2 * trackingShotting_pause_double;
                    double turn_x = b.Position.X + 200 * Math.Sin(angle) * trackingShotting_pause_double;
                    double turn_y = b.Position.Y - 200 * Math.Cos(angle) * trackingShotting_pause_double;
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
                    new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(.1f), new Vector2((float)f_x, (float)f_y), shotting_point);

                    //bullet #2
                    new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(.1f), new Vector2((float)((Math.Sqrt(3) / 2) * f_x), (float)((-.5) * f_y)), shotting_point);

                    //bullet #3
                    new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(.1f), new Vector2((float)(-(Math.Sqrt(3) / 2) * f_x), (float)((-.5) * f_y)), shotting_point);
                }
            }

            if (trackingShooting_pause > TimeSpan.FromSeconds(8))
            {
                trackingShooting_pause = TimeSpan.Zero;
            }
        }

        public override void Shoot(GameEngineBehaviors b)
        {
            trackingBullet_mode2(b);
        }
    }
}
