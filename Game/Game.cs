using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using BH_STG;

namespace TeamWowGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        /*
         * Really want this game to be Tile-Based but don't know how to do. Anyone wanna volunteer?
         */

        //------------------------------------------------------------------------  

        public Game()
        {
            setGraphic();
            Content.RootDirectory = "Content";
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GameEngine.spriteBatch = new SpriteBatch(GraphicsDevice);
            Paths.Load = _Load;
            Paths.GenerateWave_FileName = _GenerateWave_FileName;
            Paths.Menu_Content = _Menu_Content;
            Paths.kmap_FileName = _kmap_FileName;
            GameEngine.menuComponent = new MenuComponent(this);
            Components.Add(GameEngine.menuComponent);
            base.Initialize();


        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            /*
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f);     
            // makes Game() call "Update()" 100 times per second 
            // set it lower if game running slow, but not too low 
            */
            this.IsFixedTimeStep = false;
            // Create a new SpriteBatch, which can be used to draw textures.

            //set game style
            _player = _player_Library[_gameStyle];
            _e1 = _e1_Library[_gameStyle];
            _fb = _fb_Library[_gameStyle];
            _midBoss = _midBoss_Library[_gameStyle];
            _butterfly = _butterfly_Library[_gameStyle];
            _lifeStar = _lifeStar_Library[_gameStyle];
            // !!! Please !!! 
            // Put all Custom value in "Settings.cs", instead of hard code it
            #region Loading Contents
            DefaultSizes.DefaultBulletSize = DefaultBulletSize;
            DefaultSizes.DefaultRivalSize = DefaultRivalSize;

            Images.BG = Content.Load<Texture2D>(_bg);
            Images.MyBullet = Content.Load<Texture2D>(_bullet1);
            Images.MyBullet_bomb = Content.Load<Texture2D>(_bomb);
            Images.EnemyBullet = Content.Load<Texture2D>(_bullet2);
            Images.Enemy1 = Content.Load<Texture2D>(_e1);

            Images.final_boss = Content.Load<Texture2D>(_fb);
            Images.Scattershot_bullet = Content.Load<Texture2D>(_sb);
            Images.tracking_Bullet = Content.Load<Texture2D>(_tb);
            Images.Scattershot_Bullet_wall = Content.Load<Texture2D>(_sbw);
            Images.MidBoss = Content.Load<Texture2D>(_midBoss);
            Images.butterfly = Content.Load<Texture2D>(_butterfly);
            Images.MidBossCompanion = Content.Load<Texture2D>(_midBossCompanion);
            Images.lifeStar = Content.Load<Texture2D>(_lifeStar);

            Sounds.MyBullet = Content.Load<SoundEffect>(_Shot);
            Sounds.BGM = Content.Load<Song>(_BGM);

            Fonts.RemainingLives = Content.Load<SpriteFont>(_RemainingLives);
            Fonts.GG = Content.Load<SpriteFont>(_GG);
            Fonts.CheatMode = Content.Load<SpriteFont>(_CheatMode);
            Fonts.EnemyLocater = Content.Load<SpriteFont>(_enemyLocater);

            PlayerAttributes.DefaultPlayerSize = DefaultPlayerSize;
            PlayerAttributes.initLives = playerInitialLives;
            PlayerAttributes.Velocity = new Vector2(PlayerXSpeed, PlayerYSpeed);
            PlayerAttributes.Image = Content.Load<Texture2D>(_player);

            CloudAttributes.Image1 = Content.Load<Texture2D>(_c1);
            CloudAttributes.Image2 = Content.Load<Texture2D>(_c2);
            CloudAttributes.Velocity = CloudVelocity;

            #endregion

            #region Background Animation and Effects Setup
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Sounds.BGM);
            MediaPlayer.Volume = _volume;

            #endregion

            #region Currently Useless fonts. Haven't started yet
            //EnemyLocater = Content.Load<SpriteFont>(_enemyLocater);
            Fonts.Score = Content.Load<SpriteFont>(_Score);
            #endregion
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GameEngine.ExitGame || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || (Keyboard.GetState().IsKeyDown(Keys.F4) && (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) || Keyboard.GetState().IsKeyDown(Keys.RightAlt))))
            {
                //generateWaves.Dispose();
                Exit();
            }

            //in game setting: adjust volume level && change game Style (Enemy and player's image) ps: changing player's image not working. 
            if ((InputManager.Instance.KeyDown(Keys.LeftShift) || InputManager.Instance.KeyDown(Keys.RightShift)))
            {
                if (InputManager.Instance.KeyPressed(Keys.Up))
                {
                    _volume += 0.2f;
                }
                else if (InputManager.Instance.KeyPressed(Keys.Down))
                {
                    _volume -= 0.2f;
                }
                else if (InputManager.Instance.KeyPressed(Keys.Right))
                {
                    _gameStyle += 1;
                    if (_gameStyle > 2)
                    {
                        _gameStyle = 0;
                    }

                    _player = _player_Library[_gameStyle];
                    _e1 = _e1_Library[_gameStyle];
                    _fb = _fb_Library[_gameStyle];
                    _midBoss = _midBoss_Library[_gameStyle];
                    _butterfly = _butterfly_Library[_gameStyle];
                    _lifeStar = _lifeStar_Library[_gameStyle];

                    // TODO: Add your update logic here
                    //GameEngine;
                    Images.Enemy1 = Content.Load<Texture2D>(_e1);
                    Images.final_boss = Content.Load<Texture2D>(_fb);
                    Images.MidBoss = Content.Load<Texture2D>(_midBoss);
                    Images.butterfly = Content.Load<Texture2D>(_butterfly);
                    Images.lifeStar = Content.Load<Texture2D>(_lifeStar);

                    GameEngine.menuComponent.ReStartGame();//.ChangeScreens("GamePlayScreen", true);

                }
                MediaPlayer.Volume = _volume;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GameEngine.spriteBatch.Begin();//Start Drawing
            GameEngine.spriteBatch.Draw(Images.BG, new Rectangle(0, 0, GameEngine.graphic.PreferredBackBufferWidth, GameEngine.graphic.PreferredBackBufferHeight), Color.White); //Drawing Background

            #region Deploy Text (fonts). Haven't started yet
            //GameEngine.spriteBatch.DrawString(EnemyLocater, "Enemy",Vector2.Zero, Color.White);
            //GameEngine.spriteBatch.DrawString(EnemyName, "Enemy Name", Vector2.Zero, Color.White);

            #endregion

            base.Draw(gameTime);

            GameEngine.spriteBatch.End();  //Ended Drawing
            // TODO: Add your drawing code here
        }

        private void setGraphic()
        {
            GameEngine.graphic = new GraphicsDeviceManager(this);
            GameEngine.graphic.SynchronizeWithVerticalRetrace = false; //make game faster
            GameEngine.graphic.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;  // set this value to the desired width of your window
            GameEngine.graphic.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;   // set this value to the desired height of your window

            GameEngine.graphic.IsFullScreen = true; //comment it out if debugging

            GameEngine.graphic.ApplyChanges();
        }
    }
}
