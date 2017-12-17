using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace BH_STG
{
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent//public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        public Vector2 Dimensions { private set; get; }
        private GameScreen currentScreen, newScreen;
        public Image Image { get; private set; }
        private Dictionary<string, GameScreen> screens = new Dictionary<string, GameScreen>();
        public bool IsTransitioning { get; private set; }
        
        void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1.0f)
                {
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;                    
                    currentScreen.LoadContent();
                    if (!screens.ContainsKey(currentScreen.Type.Name))
                    {
                        screens[currentScreen.Type.Name] = currentScreen;
                    }
                }
                else if (Image.Alpha == 0.0f)
                {
                    Image.IsActivate = false;
                    IsTransitioning = false;
                }
            }
        }
        public void ReStartGame()
        {
            if (screens.ContainsKey("GamePlayScreen"))
            {
                (screens["GamePlayScreen"] as GamePlayScreen).ReStart();
            }
            ChangeScreens("GamePlayScreen");
        }

        public void ChangeScreens(string screenName)
        {
            if (screens.ContainsKey(screenName))
            {
                newScreen = screens[screenName];// 
            }
            else
            {
                newScreen = (GameScreen)Activator.CreateInstance(Type.GetType(typeof(MenuComponent).Namespace + "." + screenName));
            }

            Image.IsActivate = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransitioning = true;
        }

        public MenuComponent(Game game) : base(game)
        {
            Image = new Image() { Path = "ScreenManager/FadeImage", Effects = "FadeEffect" };
            new Content_ItemsNeedtoLoadLocalMedias(Game.Content);
            Dimensions = new Vector2(GameEngine.graphic.PreferredBackBufferWidth, GameEngine.graphic.PreferredBackBufferHeight);
            currentScreen = new MenuScreen();
            screens["MenuScreen"] = currentScreen;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            currentScreen.LoadContent();
            Image.LoadContent();
            Image.ToFullScreen();
        }

        protected override void UnloadContent()
        {
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPressed(Keys.Enter) && currentScreen.Type.Name == "GamePlayScreen")
            {
                currentScreen.UnloadContent();
                ChangeScreens("MenuScreen");
            }

            Transition(gameTime);
            currentScreen.Update(gameTime);
            base.Update(gameTime);           
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            currentScreen.Draw();
            if (IsTransitioning)
            {
                Image.Draw();
            }
        }
    }
}