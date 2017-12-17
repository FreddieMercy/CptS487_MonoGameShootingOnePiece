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
    public abstract class Character : GameEngineBehaviors
    {
        public int Lives;
        public Character(Texture2D I, Vector2 S = default(Vector2), IBehaviors b=null,  Vector2 P = default(Vector2), int _Lives = 1, Vector2 V = default(Vector2)) : base(I,S,b, P, V)
        {
            UpdateLives(_Lives);
        }
        public Character(Entity e, Vector2 P = default(Vector2)) : base(e, P)
        {
            UpdateLives(e._Lives);
        }
        protected void UpdateLives(int x)
        {
            if (x <= 0)
            {
                throw new NullReferenceException("Number of lives is not Positive!"); // not only solve potential bugs but also can signal other parties that it died ( important: hitted != died )
            }
            Lives = x;
        }

        protected override void UpdateBoard()
        {
            CollisionDetect(boardA);
        }

        protected override void BoardLogic(int X, int Y)
        {
            if (boardA[X, Y] != this)
            {
                if (CheckBoardSlotEmpty(X, Y))
                {
                    boardA[X, Y] = this;
                }
                else
                {
                    Dispose();
                }
            }
        }
        
        protected virtual bool CheckBoardSlotEmpty(int X, int Y)
        {
            return boardA[X, Y] == null || boardA[X, Y].isDisposed;
        }
    }
}
