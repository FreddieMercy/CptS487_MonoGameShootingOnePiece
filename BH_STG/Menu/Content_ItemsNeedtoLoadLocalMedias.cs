using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BH_STG
{
    public class Content_ItemsNeedtoLoadLocalMedias//: Microsoft.Xna.Framework.DrawableGameComponent//public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        private static Dictionary<string, object> table = new Dictionary<string, object>();
        private static ContentManager Content;

        public Content_ItemsNeedtoLoadLocalMedias(ContentManager c = null)
        {
            if (c != null && c != Content)
            {
                Content = c;
            }
        }

        protected T Load<T>(string assetName)
        {
            if (table.ContainsKey(assetName))
            {
                return (T)table[assetName];
            }

            table[assetName] = Content.Load<T>(assetName);
           
            return (T)table[assetName];
        }
        
        protected void Unload()
        {
        }
    }
}
