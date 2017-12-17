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
    public interface IBehaviors
    {
        Vector2 Move(GameEngineBehaviors b, Vector2 V, List<GameEngine> A);
    }

    public interface itIsAnObject
    {
        GameEngine BelongsTo();
    }


    public enum Flags
    {
        Player = 1,
        None = 0,
        Enemy = -1
    }
    public abstract class Attack
    {
        public virtual void Shoot(GameEngineBehaviors b)
        {
        }
    }

    public class Movement : IBehaviors
    {
        protected Attack attacks;

        public Movement(Attack a = null)
        {
            attacks = a;
        }

        public virtual Vector2 Move(GameEngineBehaviors b, Vector2 V, List<GameEngine> A)
        {
            if (attacks != null)
            {
                attacks.Shoot(b);
            }
            return b.Position + V;
        }
    }
}