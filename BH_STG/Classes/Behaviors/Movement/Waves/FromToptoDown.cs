using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
namespace BH_STG
{
    class FromToptoDown : RegularEnemyBehavior
    {
        private TimeSpan timer = TimeSpan.Zero;
        private Vector2 direction = new Vector2((float)0, (float)1);
        public override Vector2 Move(GameEngineBehaviors b, Vector2 V, List<GameEngine> A)
        {
            base.Move(b, V, A);
            timer += GameEngine.gameTime.ElapsedGameTime;
            
            direction.Y = 1f;
            

            return b.Position + direction;
        }
    }
}
