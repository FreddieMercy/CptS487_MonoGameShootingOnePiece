using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BH_STG
{
    public class MenuScreen : GameScreen
    {
        public Image Image;
        MenuManager menuManager;
        public MenuScreen()
        {
            menuManager = new MenuManager(this);
            Image = new Image() { Path = "SplashScreen/Image", Effects = "FadeEffect", IsActivate=true, Alpha=0.5f};
            Image.LoadContent();
            Image.ToFullScreen();
            Image.FadeEffect.FadeSpeed = 0.05f;
        }
        public override void LoadContent()
        {
            base.LoadContent();
            menuManager.LoadContent("Menus/TitleMenu.xml");
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
            menuManager.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);
            menuManager.Update(gameTime);
        }
        public override void Draw()
        {
            Image.Draw();
            menuManager.Draw();
        }
    }
}