using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KmapInterface;
using System.IO;

namespace BH_STG
{
    public class PlayerOperation : Player
    {
        //public Image Image; //Not for now
        private Dictionary<string, Keys> keymap;

        public PlayerOperation(Texture2D I, Vector2 S = default(Vector2), int _Lives = 1, Vector2 V = default(Vector2), IBehaviors b = null) : base(I, S, _Lives, V, b)
        {
            LoadContent();
        }

        /*
        public PlayerOperation(Entity) : base()
        {
        }             
             */
             
        protected override void Move()
        {
            if (keymap == null)
            {
                //if no key was binded, use old. If even just one key was binded, should use new.
                //User should have option to "unset" keys
                base.Move();
            }
            else
            {
                Vector2 V = getSpeed;
                Vector2 movement = Vector2.Zero;

                //forgot to set "slow mode" in the kmap window... I am sorry
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))//slow motion
                {
                    V /= 2;
                }
                Vector2 PlayerLocation = this.Position;
                if (keymap.ContainsKey("Left")&& Keyboard.GetState().IsKeyDown(keymap["Left"]))
                {
                    if (PlayerLocation.X - V.X >= 0)
                    {
                        movement.X -= V.X;
                    }
                    else
                    {
                        movement.X -= PlayerLocation.X;
                    }
                }
                if (keymap.ContainsKey("Right") && Keyboard.GetState().IsKeyDown(keymap["Right"]))
                {
                    if (PlayerLocation.X + V.X <= graphic.PreferredBackBufferWidth - Size.X)
                    {
                        movement.X += V.X;
                    }
                    else
                    {
                        movement.X += graphic.PreferredBackBufferWidth - Size.X - PlayerLocation.X;
                    }
                }
                if (keymap.ContainsKey("Up") && Keyboard.GetState().IsKeyDown(keymap["Up"]))
                {
                    if (PlayerLocation.Y - V.Y >= 0)
                    {
                        movement.Y -= V.Y;
                    }
                    else
                    {
                        movement.Y -= PlayerLocation.Y;
                    }
                }
                if (keymap.ContainsKey("Down") && Keyboard.GetState().IsKeyDown(keymap["Down"]))
                {
                    if (PlayerLocation.Y + V.Y <= graphic.PreferredBackBufferHeight - Size.Y)
                    {
                        movement.Y += V.Y;
                    }
                    else
                    {
                        movement.Y += graphic.PreferredBackBufferHeight - Size.Y - PlayerLocation.Y;
                    }
                }
                this.UpdatePosition(PlayerLocation + movement);
            }
        }
        protected override void Attack()
        {
            if (keymap == null)
            {
                base.Attack();
            }
            else
            {
                gametimepassed += GameEngine.gameTime.ElapsedGameTime;

                if (keymap.ContainsKey("Shoot") && Keyboard.GetState().IsKeyDown(keymap["Shoot"]))
                {
                    if (gametimepassed > TimeSpan.FromMilliseconds(30))
                    {
                        gametimepassed = TimeSpan.Zero;
                        //each single bullet is an individual in-game-object. it knows what it should do
                        new Bullet(this, Images.MyBullet, DefaultSizes.DefaultBulletSize, new PlayerBulletBehavior(), new Vector2(0, -20));
                    }
                }
            }
        }

        public void LoadContent()
        {
            //Image.LoadContent();
            try
            {
                Dictionary<string, Keys> k = new Dictionary<string, Keys>();
                Dictionary<string, string> tmp = Extensions.FromJsonToDictionary(File.ReadAllText(Paths.Load + Paths.kmap_FileName));
                foreach (string key in tmp.Keys)
                {
                    Keys matched = getMatchedKey(tmp[key]);
                    if (matched != Keys.None)
                    {
                        k[key] = matched;
                    }
                }

                if (k.Count > 0)
                {
                    keymap = k;
                }
                else
                {
                    UnloadContent();
                }
            }
            catch (Exception)
            {
                UnloadContent();
            }
        }

        public void UnloadContent()
        {
            //Image.UnloadContent();
            keymap = null;
        }

        private Keys getMatchedKey(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
            return Keys.None;
            }
            int a = s.ToCharArray()[0];
            return (Keys.None+(int)s[0]);
        }
        
        /*
        public void Update(GameTime gameTime)
        {
            Image.IsActivate = true;
            if (Velovity.X == 0)
            {
                if (InputManager.Instance.KeyDown(Keys.Down))
                { 
                    Velovity.Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 0;
                }
                else if (InputManager.Instance.KeyDown(Keys.Up))
                {
                    Velovity.Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 3;
                }
                else
                    Velovity.Y = 0;
            }

            if (Velovity.Y == 0)
            {
                if (InputManager.Instance.KeyDown(Keys.Right))
                {
                    Velovity.X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 2;
                }
                else if (InputManager.Instance.KeyDown(Keys.Left))
                {
                    Velovity.X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 1;
                }
                else
                    Velovity.X = 0;
            }

            if (Velovity.X == 0 && Velovity.Y == 0)
                Image.IsActivate = false;

            Image.Update(gameTime);
            Image.Position += Velovity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
        */
    }
}