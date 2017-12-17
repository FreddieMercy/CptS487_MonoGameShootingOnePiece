using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;

namespace BH_STG
{
    public class Player : Character
    {
        protected TimeSpan gametimepassed = TimeSpan.Zero;
        public bool JustRevived { get; private set; }
        public bool ImmuDamage { get; private set; }
        private UpdateTimer Timer = new UpdateTimer(TimeSpan.FromSeconds(3));
        private UpdateTimer ScoreTimer = new UpdateTimer(TimeSpan.FromSeconds(1));
        public Player(Texture2D I, Vector2 S = default(Vector2), int _Lives = 1, Vector2 V = default(Vector2), IBehaviors b = null) : base(I, S, b, default(Vector2), _Lives, V)
        {
            ScoreTimer.Start();
            JustRevived = false;
            this.UpdatePosition(new Vector2((graphic.PreferredBackBufferWidth - Size.X) / 2, graphic.PreferredBackBufferHeight - Size.Y));
        }
        public override void Dispose()
        {
            try
            {
                if (!JustRevived)
                {
                    UpdateLives(Lives - 1);
                    Rez();
                }
            }
            catch (Exception)
            {
                //Game Over;
                base.Dispose();
                Exit = true;
            }
        }
        protected override void Action()
        {
            if (Timer.TimeUp(GameEngine.gameTime))
            {
                JustRevived = false;
                Timer.Stop();
            }
            Move(); //user control            
            Attack();
            OtherActions();
            base.Action();

            if (!JustRevived)
            {
                if (ScoreTimer.TimeUp(GameEngine.gameTime))
                {
                    incScore();
                }
            }
        }
        protected virtual void OtherActions()
        {
            ImmuDamage = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                ImmuDamage = true;
            }
        }
        protected virtual void Attack()
        {
            gametimepassed += GameEngine.gameTime.ElapsedGameTime;

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                if (gametimepassed > TimeSpan.FromMilliseconds(30))
                {
                    gametimepassed = TimeSpan.Zero;
                    //each single bullet is an individual in-game-object. it knows what it should do
                    new Bullet(this, Images.MyBullet, DefaultSizes.DefaultBulletSize, new PlayerBulletBehavior(), new Vector2(0, -20));
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                if (gametimepassed > TimeSpan.FromSeconds(2))
                {
                    gametimepassed = TimeSpan.Zero;
                    
                    while(Arena.Count()>4)
                    {
                        Arena.RemoveAt(Arena.Count() - 1);
                    }
                    //each single bullet is an individual in-game-object. it knows what it should do
                    new Bullet(this, Images.MyBullet_bomb, DefaultSizes.DefaultBulletSize, new PlayerBulletBehavior(), new Vector2(0, -1));
                }
            }
        }
        protected virtual void Move()
        {
            Vector2 V = getSpeed;
            Vector2 movement = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))//slow motion
            {
                V /= 2;
            }
            Vector2 PlayerLocation = this.Position;
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
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
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
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
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
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
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
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

        private void Rez()//Only "Player" can "Rez()". Which means only "Player" have special action in the process of "hit->Revive"
        {
            //Should immune the damage for short period of time
            //But I haven't got time to do that
            //Therefore, for now, if Player was hitted when it was just revived, game will be exited right way because all its lives had been consumed by the logic loop in "BoardLogic(int X, int Y)".  
            JustRevived = true;
            Score /= Lives;
            Timer.Start();
            Vector2 origin = new Vector2((graphic.PreferredBackBufferWidth - Size.X) / 2, graphic.PreferredBackBufferHeight - Size.Y);
            if (Position.X != origin.X || Position.Y != origin.Y)
            {
                this.UpdatePosition(origin);
            }
        }

        protected override void UpdateBoard()
        {
            if (boardA!=null && !JustRevived && !ImmuDamage)
            {
                BoardLogic((int)Math.Floor((Center.X - getHalfHitBox.X) / (getHalfHitBox.X * 2)), (int)Math.Floor((Center.Y - getHalfHitBox.Y) / (getHalfHitBox.Y * 2)));
            }
        }

        protected override bool CheckBoardSlotEmpty(int X, int Y)
        {
            return base.CheckBoardSlotEmpty(X, Y) && (boardB == null || boardB[X, Y] != Flags.Enemy);
        }
    }
}
