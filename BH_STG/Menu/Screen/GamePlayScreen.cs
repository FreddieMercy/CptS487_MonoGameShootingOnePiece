using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.GamerServices;

namespace BH_STG
{
    public partial class GamePlayScreen : GameScreen
    {
        //PlayerOperation player;
        private bool running = false;
        private GenerateWaves generateWaves;// = new GenerateWaves();

        public override void LoadContent()
        {
            base.LoadContent();
            //XmlManager<PlayerOperation> playerLoader = new XmlManager<PlayerOperation>();
            //player = playerLoader.Load(Paths.Load+"Gameplay/Player.xml"); //should load everything except keymap
            //player.LoadContent();  
            running = true;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            //player.UnloadContent();
            running = false;
        }

        public GamePlayScreen()
        {
            ReStart();
        }

        public void ReStart()
        {
            GameEngine.clearItems();
            new Cloud(CloudAttributes.Image1, new Vector2(GameEngine.graphic.PreferredBackBufferWidth / 2, GameEngine.graphic.PreferredBackBufferHeight / 2), CloudAttributes.Velocity);
            new Cloud(CloudAttributes.Image2, new Vector2(GameEngine.graphic.PreferredBackBufferWidth / 2, GameEngine.graphic.PreferredBackBufferHeight / 2), CloudAttributes.Velocity, new Vector2(GameEngine.graphic.PreferredBackBufferWidth / 2, GameEngine.graphic.PreferredBackBufferHeight / 2));
            //generateWaves.Dispose();
            generateWaves = new GenerateWaves();
            //GameEngine.Player = new PlayerOperation(Content.Load<Texture2D>(_player), DefaultSizes.DefaultPlayerSize, playerInitialLives, new Vector2(PlayerXSpeed, PlayerYSpeed));
            GameEngine.Player = new PlayerOperation(PlayerAttributes.Image, PlayerAttributes.DefaultPlayerSize, PlayerAttributes.initLives, PlayerAttributes.Velocity);
            (GameEngine.Player as PlayerOperation).LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

            //player.Update(gameTime);
            if (running)
            {
                generateWaves.Start(gameTime);
                GameEngine.gameTime = gameTime;
                GameEngine.actItems();
            }
        }

        public override void Draw()
        {
            //player.Draw(spriteBatch);

            GameEngine.getItems();

            #region Texts
            GameEngine.spriteBatch.DrawString(Fonts.CheatMode, "Press 'Tab' to immute damage ", new Vector2(GameEngine.graphic.PreferredBackBufferWidth - Fonts.CheatMode.MeasureString("Press 'Tab' to immute damage ").X, 0), Color.White);
            GameEngine.spriteBatch.DrawString(Fonts.EnemyLocater, "Press 'Shift + Right Arrow' to change theme, press 'Shift + Up/Down Arrow' to adjust volume", new Vector2(GameEngine.graphic.PreferredBackBufferWidth - Fonts.EnemyLocater.MeasureString("Press 'Shift + Right Arrow' to change theme, press 'Shift + Up/Down Arrow' to adjust volume   ").X, 50), Color.White);

            GameEngine.spriteBatch.DrawString(Fonts.RemainingLives, "Remaining Lives: ", Vector2.Zero, Color.White);// + char.ConvertFromUtf32(8595).ToString()+" * " + ((GameEngine.Player.isDisposed)?(GameEngine.Player.Lives-1).ToString(): GameEngine.Player.Lives.ToString()), Vector2.Zero, Color.White);
            GameEngine.spriteBatch.DrawString(Fonts.Score,"Score: "+ GameEngine.Score.ToString(),new Vector2(0, 20), Color.LightGoldenrodYellow);
            for (int i = 0; i < ((GameEngine.Player.isDisposed) ? (GameEngine.Player.Lives - 1) : GameEngine.Player.Lives); ++i)
            {
                GameEngine.spriteBatch.Draw(Images.lifeStar, new Rectangle(i * 20 + (int)Fonts.RemainingLives.MeasureString("Remaining Lives: ").X, 0, 18, 18), Color.White);
            }

            if (GameEngine.Exit)
            {
                GameEngine.spriteBatch.DrawString(Fonts.GG, "Game Over", new Vector2((GameEngine.graphic.PreferredBackBufferWidth - Fonts.GG.MeasureString("Game Over").X) / 2, (GameEngine.graphic.PreferredBackBufferHeight - Fonts.GG.MeasureString("Game Over").Y) / 2), Color.White);
            }
            else
            {
                if (generateWaves.End && GameEngine.End)
                {
                    GameEngine.spriteBatch.DrawString(Fonts.GG, "You Won!! LOL~", new Vector2((GameEngine.graphic.PreferredBackBufferWidth - Fonts.GG.MeasureString("You Won!! LOL~").X) / 2, (GameEngine.graphic.PreferredBackBufferHeight - Fonts.GG.MeasureString("You Won!! LOL~").Y) / 2), Color.White);
                }
            }
            #endregion
        }
    }
}
