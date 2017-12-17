using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KmapInterface;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace BH_STG
{
    public class MenuManager
    {
        Menu menu;
        public bool isTransitioning;
        private MainWindow kmap;
        private MenuScreen parent;
        void Transition(GameTime gameTime)
        {
            if (isTransitioning)
            { 
                for (int i = 0; i < menu.Items.Count; i++)
                {
                    menu.Items[i].Image.Update(gameTime);
                    float first = menu.Items[0].Image.Alpha;
                    float last = menu.Items[menu.Items.Count - 1].Image.Alpha;
                    if (first == 0.0f && last == 0.0f)
                    {
                        menu.ID = menu.Items[menu.ItemNumber].LinkID;
                    }
                    else if (first == 1.0f && last == 1.0f)
                    {
                        isTransitioning = false;
                        foreach (MenuItem item in menu.Items)
                        {
                            item.Image.RestoreEffect();
                        }
                    }
                }
            }
        }

        public MenuManager(MenuScreen p)
        {
            parent = p;
            menu = new Menu();
            menu.OnMenuChange += menu_OnMenuChange;
        }
        void menu_OnMenuChange(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(menu.ID))
            {
                XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
                menu.UnloadContent();
                menu = xmlMenuManager.Load(Paths.Load + menu.ID);
                menu.LoadContent();
                menu.OnMenuChange += menu_OnMenuChange;
                menu.Transition(0.0f);

                foreach (MenuItem item in menu.Items)
                {
                    item.Image.StoreEffects();
                    item.Image.ActivateEffect("FadeEffect");
                }
            }
        }
        public void LoadContent(string menuPath)
        {
            if (menuPath != String.Empty)
            {
                menu.ID = menuPath;
            }
        }
        public void UnloadContent()
        {
            menu.UnloadContent();
        }
        public void Update(GameTime gameTime)
        {
            if (!isTransitioning)
            {
                menu.Update(gameTime);
            }
            if (InputManager.Instance.KeyPressed(Keys.Enter) && !isTransitioning )
            {
                if (kmap != null)
                {
                    kmap.Close();
                    kmap = null;
                }
                switch (menu.Items[menu.ItemNumber].LinkType)
                {
                    case "Screen":
                        GameEngine.menuComponent.ChangeScreens(menu.Items[menu.ItemNumber].LinkID);
                        break;
                    case "Function":
                        this.Functions(menu.Items[menu.ItemNumber].LinkID);
                        break;
                    default:
                        isTransitioning = true;
                        menu.Transition(1.0f);
                        foreach (MenuItem item in menu.Items)
                        {
                            item.Image.StoreEffects();
                            item.Image.ActivateEffect("FadeEffect");
                        }
                        break;
                }
            }
            Transition(gameTime);
        }
        public void Draw()
        {
            menu.Draw();
        }
        
    private void Functions(string func)
    {
        switch (func)
        {
            case "KeyMap":

                kmap = new MainWindow(Paths.Load + Paths.kmap_FileName); //I hate to design complicate stuff ... -- Freddie
                kmap.PropertyChanged += (s, e) => {

                    if (e.PropertyName == "Submit")
                    {
                        if (GameEngine.Player != null)
                        {
                            (GameEngine.Player as PlayerOperation).LoadContent();
                        }
                    }
                };
                bool isFullScreen = GameEngine.graphic.IsFullScreen;
                kmap.Closed += (s, e) => {
                    GameEngine.graphic.IsFullScreen = isFullScreen; //it works... WTF -- Freddie
                    GameEngine.graphic.ApplyChanges();
                };
                GameEngine.graphic.IsFullScreen = false;
                GameEngine.graphic.ApplyChanges();
                kmap.Show();
                isTransitioning = true;
                break;

            case "ExitMenu":
                GameEngine.ExitGame = true;
                break;
          
            case "NewGame":
                    GameEngine.menuComponent.ReStartGame();//.ChangeScreens("GamePlayScreen", true);
                    menu.Transition(1.0f);
                    foreach (MenuItem item in menu.Items)
                    {
                        item.Image.ActivateEffect("FadeEffect");
                    }
                    parent.UnloadContent();
                    break;
            default:
                isTransitioning = true;
                menu.Transition(1.0f);
                foreach (MenuItem item in menu.Items)
                {
                    item.Image.StoreEffects();
                    item.Image.ActivateEffect("FadeEffect");
                }
                break;
        }
    }
}
}
