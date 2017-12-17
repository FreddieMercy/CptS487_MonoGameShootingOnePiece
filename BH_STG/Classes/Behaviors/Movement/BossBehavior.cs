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
    //stage 1
    //scatterBullet(b);
    //trackingBullet(b);

    //stage 2 (not done yet)
    //scatterBullet_mode2(b);
    //scatterBullet_mode3(b);

    //stage 3
    //scatterBullet(b);
    //trackingBullet_mode2(b);

    //stage 4
    //randomBullet(b);
    //wallBullet(b);

    public class BossBehavior : Movement
    {
        private TimeSpan timer = TimeSpan.Zero;  //move periodically
        private Vector2 direction = new Vector2(0, 0); //current location & next direction
        private Queue<Attack> stages;
        private int lifeSpan = -1;
        private TimeSpan interval;
        private TimeSpan tracker = TimeSpan.Zero;

        private bool inPlace = false;
        private Vector2 Place = new Vector2(GameEngine.graphic.PreferredBackBufferWidth / 2, 300);

        public BossBehavior(int totalLives, TimeSpan Interval, Queue<Attack> s)
        {
            stages = s;
            updateNextTime(totalLives);
            interval = Interval;
            if (lifeSpan <= 0||interval <= TimeSpan.Zero)
            {
                throw new Exception("Changing stage too frequent!!");
            }
        }

        public override Vector2 Move(GameEngineBehaviors b, Vector2 V, List<GameEngine> A)
        {
            timer += GameEngine.gameTime.ElapsedGameTime;
            if (!inPlace)
            {
                Vector2 movement = b.Position + new Vector2(0, V.X + V.Y);
                if (movement.Y>Place.Y)
                {
                    timer = TimeSpan.Zero;
                    inPlace = true;
                    return b.Position;
                }

                return movement;
            }
            else
            {
                Random rd = new Random();
                float rd_num = (rd.Next(0, 100)) / 300f; // need random number for final boss move

                tracker += GameEngine.gameTime.ElapsedGameTime;

                if (timer < TimeSpan.FromSeconds(2))
                {
                    direction.X = -.3f + rd_num;
                    direction.Y = .3f - rd_num;
                }

                if (timer >= TimeSpan.FromSeconds(2) && timer < TimeSpan.FromSeconds(4))
                {
                    direction.X = .3f - rd_num;
                    direction.Y = .3f - rd_num;
                }

                if (timer >= TimeSpan.FromSeconds(4) && timer < TimeSpan.FromSeconds(6))
                {
                    direction.X = .3f - rd_num;
                    direction.Y = -.3f + rd_num;
                }

                if (timer >= TimeSpan.FromSeconds(6) && timer < TimeSpan.FromSeconds(8))
                {
                    direction.X = -.3f + rd_num;
                    direction.Y = -.3f + rd_num;
                }

                if (timer >= TimeSpan.FromSeconds(8))
                {
                    timer = TimeSpan.Zero; //reset timespan to 0
                }

                if ((b.Position.X <= 100 || b.Position.X >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100))
                {
                    return new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2, b.Position.Y);
                }

                if ((b.Position.Y <= 0 || b.Position.Y >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    return new Vector2(b.Position.X, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 4);
                }

                if ((b as Character).Lives <= lifeSpan || tracker >= interval)
                {
                    //Next Stage
                    stages.Dequeue();
                    tracker = TimeSpan.Zero;
                    updateNextTime(lifeSpan);
                }
                if (stages.Count > 0)
                {
                    //current stage behavior
                    stages.Peek().Shoot(b);
                }
                else
                {
                    //all stages done, boss dies
                    b.Die();
                }
                return new Vector2(b.Position.X + direction.X, b.Position.Y + direction.Y);
            }
        }

        private void updateNextTime(int totalLives)
        {
            if (stages.Count > 0)
            {
                lifeSpan = totalLives - (totalLives / stages.Count() - 1);
            }
        }

    }
}
