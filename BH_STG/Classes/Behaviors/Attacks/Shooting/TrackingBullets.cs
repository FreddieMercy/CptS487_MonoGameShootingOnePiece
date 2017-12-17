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
    public class TrackingBullets : Attack
    {
        #region Track

        private TimeSpan trackingShotting = TimeSpan.Zero;
        private TimeSpan trackingShotting_pause = TimeSpan.Zero;
        private void trackingBullet(GameEngineBehaviors b)
        {
            trackingShotting_pause += GameEngine.gameTime.ElapsedGameTime;
            string trackingShotting_pause_string = trackingShotting_pause.TotalSeconds.ToString();
            double trackingShotting_pause_double = 0;
            double.TryParse(trackingShotting_pause_string, out trackingShotting_pause_double);

            if (trackingShotting_pause < TimeSpan.FromSeconds(1))
            {
                trackingShotting += GameEngine.gameTime.ElapsedGameTime;
                if (trackingShotting > TimeSpan.FromSeconds(0.02))
                {
                    trackingShotting -= TimeSpan.FromSeconds(0.02);
                    //shot point #1
                    double angle = Math.PI * 3 * trackingShotting_pause_double;
                    double turn_x = b.Position.X + 170 * Math.Sin(angle) * trackingShotting_pause_double;
                    double turn_y = b.Position.Y - 170 * Math.Cos(angle) * trackingShotting_pause_double;
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

                    //shot point #2
                    angle = Math.PI * 3 * trackingShotting_pause_double;
                    turn_x = b.Position.X - 170 * Math.Sin(angle) * trackingShotting_pause_double;
                    turn_y = b.Position.Y + 170 * Math.Cos(angle) * trackingShotting_pause_double;
                    shotting_point = new Vector2((float)turn_x, (float)turn_y);

                    //bullet move #2
                    f_x = 0;
                    f_y = 0;
                    target_x = GameEngine.Player.Position.X;
                    target_y = GameEngine.Player.Position.Y;
                    shot_x = turn_x;
                    shot_y = turn_y;
                    f_x = (target_x - shot_x) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));
                    f_y = (target_y - shot_y) / Math.Sqrt((target_x - shot_x) * (target_x - shot_x) + (target_y - shot_y) * (target_y - shot_y));

                    //bullet #2
                    new Bullet(b, Images.tracking_Bullet, new Vector2(15, 25), new EnemyBulletBehavior(.1f), new Vector2((float)f_x, (float)f_y), shotting_point);
                }
            }

            if (trackingShotting_pause > TimeSpan.FromSeconds(4))
            {
                trackingShotting_pause = TimeSpan.Zero;
            }
        }

        #endregion
        public override void Shoot(GameEngineBehaviors b)
        {
            this.trackingBullet(b);
        }
    }
}
