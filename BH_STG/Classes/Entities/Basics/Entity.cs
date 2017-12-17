using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH_STG
{
    public class Entity
    {
        public int id = 0;
        public Texture2D I;//image 
        public Vector2 S;//size 
        public Vector2 V;//velocity in BasicWithVelocity class 
        public IBehaviors b = null; //Behavior
        public int _Lives = 1;//lives

        public Entity() { }

        public Entity(int _id, Texture2D _I, Vector2 _S, Vector2 _V, IBehaviors _b, int __Lives)
        {
            id = _id;
            I = _I;
            S = _S;
            V = _V;
            b = _b;
            _Lives = __Lives;
        }
    }
}
