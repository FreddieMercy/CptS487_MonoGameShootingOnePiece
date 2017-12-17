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
    public class Cloud : GameEngineVelocity
    {
        //Just a background animation
        private Vector2 origin;
        public Cloud(Texture2D I, Vector2 S, Vector2 V = default(Vector2), Vector2 P = default(Vector2)) : base(I, S, P, V)
        {
            origin = new Vector2(P.X, -S.Y+1);
        }
        protected override void Action()
        {
            UpdatePosition(Position + getSpeed);
            if (outOfBoundary())
            {
                UpdatePosition(origin);
            }
            base.Action();
        }
    }
}
