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
    public class Bullet : GameEngineBehaviors, itIsAnObject
    {
        private GameEngine blong;
        public readonly Flags Flag;
        //public Flags Flag { get { return this.flag; } }
        public Bullet(GameEngine B, Texture2D I, Vector2 S, IBehaviors b, Vector2 V = default(Vector2), Vector2 P = default(Vector2)) : base(I, S, b, P, V)
        {
            /*  Need to know the following:
             *      1. Each bullet is "Bullet" !!! it knows what to do
             *      2. check if hit anything by static "board"
             *      3. inventing new kinds of bullets by inventing new "IBehaviors"
             *      4. each bullet knows the unique Character/Owner it belongs to
             *      5. its velocity can be zero (making barrier)
             */

            // find its launching point
            if (B is Player)
            {
                this.Flag = Flags.Player;
                UpdatePosition(new Vector2(B.Position.X + (B.Size.X - Size.X) / 2, B.Position.Y));
            }
            else if (P == default(Vector2))
            {
                this.Flag = Flags.Enemy;
                UpdatePosition(new Vector2(B.Position.X + (B.Size.X - Size.X) / 2, B.Position.Y + I.Height));
            }
            else
            {
                this.Flag = Flags.Enemy;
                UpdatePosition(P);
            }
            blong = B;

            if (b == null)
            {
                UpdateBehavior(new Movement());
            }
        }

        public GameEngine BelongsTo()
        {
            return blong;
        }
        
        protected override void UpdateBoard()
        {
            CollisionDetect(boardB);
        }

        protected override void BoardLogic(int X, int Y)
        {
            if (Flag != boardB[X, Y])
            {
                boardB[X, Y] = Flag; //later bullet override older bullet.

                if (boardA != null && boardA[X, Y] != null && !boardA[X, Y].isDisposed)
                {
                    if (boardA[X, Y] == GameEngine.Player)
                    {
                        if (Flag == Flags.Enemy)
                        {
                            GameEngine.Player.Dispose();
                        }
                    }
                    else
                    {
                        if (Flag == Flags.Player)
                        {
                            boardA[X, Y].Dispose();
                            incScore();
                        }
                    }
                }
            }
        }
    }
}