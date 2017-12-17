using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BH_STG
{
    public class GameScreen //: Content_ItemsNeedtoLoadLocalMediass
    {
        [XmlIgnore]
        public Type Type;

        public string XmlPath;

        public GameScreen()
        {
            Type = this.GetType();
            XmlPath = Paths.Load + "Screen/" + Type.ToString().Replace(typeof(MenuComponent).Namespace + ".", "") + ".xml";
        }

        public virtual void LoadContent()
        {
            //content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            //content = ScreenManager.Instance.Content;
        }

        public virtual void UnloadContent()
        {
            //Unload();
        }
        public virtual void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
        }
        public virtual void Draw()
        {

        }
    }
}