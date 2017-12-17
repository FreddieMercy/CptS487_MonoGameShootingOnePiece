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
using System.Windows.Threading;
using System.Threading;

namespace BH_STG
{
    public class BulletScatterTwo : Bullet
    {
        private TimeSpan Timer = TimeSpan.Zero;
        private bool rotate = false;
        
        private readonly TimeSpan interval = TimeSpan.FromSeconds(2);
        private readonly TimeSpan onterval = TimeSpan.FromSeconds(1f);
        private readonly float degree = 30;
        private SoundEffectInstance i = Sounds.MyBullet.CreateInstance();
 
        public BulletScatterTwo(GameEngineBehaviors b, Vector2 V, float v = .6f) : base(b, Images.MyBullet, DefaultSizes.DefaultBulletSize, null, V)
        {
            i.IsLooped = false;
            i.Volume = v;
        }

        protected override void Action()
        {
            Timer += GameEngine.gameTime.ElapsedGameTime;
            if (i!=null)
            {
                i.Play();
                i = null;
            }
            if (!rotate)
            {
                if (Timer >= interval)
                {
                    Timer = TimeSpan.Zero;
                    rotate = !rotate;
                }
                UpdatePosition(Position+getSpeed);
                return;
            }
            if (Timer >= onterval)
            {
                Timer = TimeSpan.Zero;
                rotate = !rotate;
                if (outOfBoundary())
                {
                    Dispose();
                }
            }
            UpdatePosition(Position - new Vector2(getSpeed.X * (float)(1 + Math.Cos(degree)) - getSpeed.Y * (float)(Math.Sin(degree)), getSpeed.X * (float)(Math.Sin(degree)) + getSpeed.Y * (float)(1 + Math.Cos(degree))));
        }
    }
}