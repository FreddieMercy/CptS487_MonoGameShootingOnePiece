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
    public class PlayerBulletBehavior : Movement
    {
        private SoundEffectInstance i = Sounds.MyBullet.CreateInstance();
        public PlayerBulletBehavior(float v = .8f)
        {
            i.IsLooped = false;
            i.Volume = v;
        }
        public override Vector2 Move(GameEngineBehaviors b, Vector2 V, List<GameEngine> A)
        {
            if (i != null)
            {
                if (!b.isDisposed)
                {
                    i.Play();
                    i = null;
                }
                else
                {
                    i.Stop();
                    i.Dispose();
                }
            }
            return b.Position + V;
        }
    }

    public class EnemyBulletBehavior : PlayerBulletBehavior
    {
        public EnemyBulletBehavior(float v = .6f):base(v)
        {
        }
    }
}
