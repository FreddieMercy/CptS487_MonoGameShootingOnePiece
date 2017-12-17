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
    public class Enemy : Character
    {
        public bool hit = false;

        public Enemy(Texture2D I, Vector2 S = default(Vector2), Vector2 P = default(Vector2), Vector2 V = default(Vector2), IBehaviors b = null,int _Lives = 1) : base(I,S,b, P, _Lives, V)
        {

        }

        public Enemy(Entity e, Vector2 P = default(Vector2)) : base(e,P)
        {

        }

        protected override void Action()
        {
            if (Behavior == null)
            {
                UpdateBehavior(new RegularEnemyBehavior());
            }
            base.Action();
        }
        public override void Dispose()
        {
            hit = !hit;
            try
            {
                UpdateLives(Lives - 1);
            }
            catch
            {
                base.Dispose();
            }
        }

        public override void Die()
        {
            while (!isDisposed)
            {
                Dispose();
            }
        }

        protected override bool CheckBoardSlotEmpty(int X, int Y)
        {
            return (boardA[X,Y] is Enemy ||base.CheckBoardSlotEmpty(X, Y)) && (boardB == null || boardB[X, Y] != Flags.Player);
        }
        protected override void BoardLogic(int X, int Y)
        {
            if (boardA[X,Y] != GameEngine.Player) {
                base.BoardLogic(X, Y);
            }
            else
            {
                GameEngine.Player.Dispose();
            }
        }
    }
}
