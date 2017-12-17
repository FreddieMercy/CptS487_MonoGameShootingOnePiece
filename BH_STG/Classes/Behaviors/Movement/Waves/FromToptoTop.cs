using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
namespace BH_STG
{
    class FromToptoTop : RegularEnemyBehavior
    {
        private TimeSpan timer = TimeSpan.Zero;
        private Vector2 direction = new Vector2((float)0, (float)1);
        public override Vector2 Move(GameEngineBehaviors b, Vector2 V, List<GameEngine> A)
        {
            base.Move(b, V, A);
            timer += GameEngine.gameTime.ElapsedGameTime;

            if(b.Position.X < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/2 &&
                direction.Y > 0)
            {
                direction.X += 0.0005f;
            }
            else if (b.Position.X >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 &&
                direction.Y > 0)
            {
                direction.X -= 0.0005f;
            }
            else if (timer <= TimeSpan.FromSeconds(5))
            {
                return b.Position;
            }
         
            if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 4 < b.Position.Y)
            {
                direction.Y -= 0.005f;
            }

            return b.Position + direction;
        }
    }
}
