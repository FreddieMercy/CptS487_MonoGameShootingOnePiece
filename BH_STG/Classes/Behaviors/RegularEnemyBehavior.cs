using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace BH_STG
{
    public class RegularAttack : Attack
    {
        private TimeSpan gametimepassed = TimeSpan.Zero;
        private int timeInterval_Second;
        public RegularAttack(int time = 2)
        {
            timeInterval_Second = time;
        }
        public override void Shoot(GameEngineBehaviors b)
        {
            gametimepassed += GameEngine.gameTime.ElapsedGameTime;
            if (gametimepassed > TimeSpan.FromSeconds(timeInterval_Second))
            {
                gametimepassed -= TimeSpan.FromSeconds(timeInterval_Second);
                new Bullet(b, Images.EnemyBullet, DefaultSizes.DefaultBulletSize, new EnemyBulletBehavior(), new Vector2(0, 1));
            }
        }
    }

    public class RegularEnemyBehavior : Movement
    {
        public RegularEnemyBehavior(Attack a = null) : base(a)
        {
            if (attacks == null)
            {
                attacks = new RegularAttack();
            }
        }
    }
}
