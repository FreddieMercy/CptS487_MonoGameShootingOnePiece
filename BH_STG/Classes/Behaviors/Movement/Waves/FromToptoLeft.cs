using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
namespace BH_STG
{
    class FromToptoLeft : RegularEnemyBehavior
    {
        private TimeSpan timer = TimeSpan.Zero;
        private Vector2 direction = new Vector2((float)0, (float)1);
        public override Vector2 Move(GameEngineBehaviors b, Vector2 V, List<GameEngine> A)
        {
            base.Move(b, V, A);
            timer += GameEngine.gameTime.ElapsedGameTime;

            if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 5 < b.Position.Y &&
                direction.X >= -1)
            {
                direction.X -= 0.005f;
            }

            if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 4 < b.Position.Y &&
                direction.Y >= 0)
            {
                direction.Y -= 0.005f;
            }
            return b.Position + direction;
        }
    }
}
