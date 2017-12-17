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
    public abstract class GameEngineBehaviors : GameEngineVelocity
    {
        protected IBehaviors Behavior { get; private set; } //extra and replaecable actions we pr-edefined
        public GameEngineBehaviors(Texture2D I, Vector2 S, IBehaviors b = null, Vector2 P = default(Vector2), Vector2 V = default(Vector2)) : base(I, S, P, V)
        {
            Behavior = b;
        }
        public GameEngineBehaviors(Entity e, Vector2 P = default(Vector2)) : base(e, P)
        {
            Behavior = e.b;
        }
        protected override void Action()
        {
            /*
                | <--- this place ---> |
                in its derive classes should implement the baic action of that class

                And then:
            */
            if (Behavior != null) //implement our pre-defined extra action
            {
                UpdatePosition(Behavior.Move(this, this.getSpeed, Arena));
            }
            base.Action(); // all overrided "Action()" should call its base.Action()
        }
        protected void UpdateBehavior(IBehaviors B)//public ?
        {
            Behavior = B;
        }
    }
    public abstract class GameEngineVelocity : GameEngine
    {
        private Vector2 Velocity; //velocity cannot be changed once it had been set, but you can use it in different ways to achieve what you want

        public GameEngineVelocity(Texture2D I, Vector2 S, Vector2 P = default(Vector2), Vector2 V = default(Vector2)) : base(I, S, P)
        {
            if (V == default(Vector2))
            {
                Velocity = new Vector2(1, 1);
            }
            else
            {
                Velocity = V;
            }
        }

        public GameEngineVelocity(Entity e, Vector2 P = default(Vector2)) : base(e, P)
        {
            if (e.V == default(Vector2))
            {
                Velocity = new Vector2(1, 1);
            }
            else
            {
                Velocity = e.V;
            }
        }

        protected virtual Vector2 getSpeed
        {
            get
            {
                return Velocity;
            }
        }

        protected void CollisionDetect<T>(T[,] board)
        {
            if (board != null)
            {
                Vector2 Area = Position + Size;
                for (int i = Math.Max(0, (int)Math.Floor(Position.X / (getHalfHitBox.X * 2))); i < Math.Min(board.GetLength(0), (int)Math.Floor(Area.X / (getHalfHitBox.X * 2))); ++i) // Hashing/Compressing
                {
                    for (int j = Math.Max(0, (int)Math.Floor(Position.Y / (getHalfHitBox.Y * 2))); j < Math.Min(board.GetLength(1), (int)Math.Floor(Area.Y / (getHalfHitBox.Y * 2))); ++j) // Hashing/Compressing
                    {
                        BoardLogic(i, j);
                    }
                }
            }
        }
        protected virtual void BoardLogic(int X, int Y)
        {
        }
    }
}
