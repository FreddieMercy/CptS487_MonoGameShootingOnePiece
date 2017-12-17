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

namespace BH_STG
{
    public abstract class GameEngine : IDisposable
    {
        /*
         * This is the most important class. Its static Property "Arena" contains all objects currently in game.
         * When derive object was constructed, it will be automatically added to "Arena"
         * All operations of all in-game objects are in this class. Therefore most Methods of "GameEngine" and its derive classes are either "private" or "protected"
         */
        #region Fields

        #region IDK What I am doing...
        public static bool ExitGame = false;
        public static bool NewGame = false;
        public static bool Exit { get; protected set; } //signal to Exit the game. Should be always "false" unless the "Player"'s number of lives is equal ro less to 0 
        public static bool End { get; private set; }
        #endregion

        public static int Score { get; protected set; }
        public static MenuComponent menuComponent;
        public static Player Player;
        public bool isDisposed { get; protected set; } //remove out-of-game objects from "Arena"
        public static GameTime gameTime; //sync with "gameTime" in  Game().Update()
        public static GraphicsDeviceManager graphic; //global graphic manager of the whole Game
        public static SpriteBatch spriteBatch; //global "drawer" of the whole Game. Use it to draw all objects in "Arena" in the scope of this class 
        public Vector2 Position { get; private set; }
        public Vector2 Size { get; private set; } //graphic size to display
        public Vector2 Center //Graphic Center of the object
        {
            get
            {
                return new Vector2(Position.X + Size.X / 2, Position.Y + Size.Y / 2);
            }
        }
        protected static List<GameEngine> Arena = new List<GameEngine>(); //Contains all IN-GAME objects. Automatically add newly constructed object
        protected static GameEngine[,] boardA; //used to track player and enemy references. refresh everytime when Game().Update() was called
                                               // board = [Game Width/ halfD.X, Game Height/halfD] everytime when refresh the board. 
                                               // The reason will be explained at "BasicAndOthers.cs"->"GameEngineBehaviors" -> "UpdateBoard()"
        protected static Flags[,] boardB;     // used to track bullets
        private static Vector2 halfD, tmpHalfD; // HALF DEMENSION of the "hitted box". It should be the half demensions of the object with the smallest size of all ever created objects 
                                                // the demensions of the "hitted box" is the smallest size of all ever created objects because they are the only reasonable values make the logical sense
        private Texture2D img; //rendered image of "this" object  
        #endregion
        #region Public
        public Texture2D Image
        {
            get
            {
                return img; //just a getter, and can ONLY be getter! Because all operations should be in this class. If want to update image, call "protected void UpdateImage(Texture2D path)"
            }
        }

        public GameEngine(Texture2D I, Vector2 S, Vector2 P = default(Vector2))
        {
            End = false;
            UpdateImage(I);
            UpdatePosition(P);
            UpdateSize(S);
            Arena.Add(this);
            isDisposed = false;
            if (tmpHalfD == default(Vector2)) // for newly created objects, record their smallest demension, and sync with halfD when next time refresh the board 
                                              // it has to be done in this way, because "halfD" cannot be changed during "UpdateBoard()"
            {
                tmpHalfD = new Vector2(Math.Max(S.X / 2, 1), Math.Max(S.Y / 2, 1));
            }
            else
            {
                tmpHalfD = new Vector2(Math.Max(Math.Min(tmpHalfD.X, S.X / 2), 1), Math.Max(Math.Min(tmpHalfD.Y, S.Y / 2), 1));
            }
        }

        public GameEngine(Entity e, Vector2 P = default(Vector2))
        {
            UpdateImage(e.I);
            UpdatePosition(P);
            UpdateSize(e.S);
            Arena.Add(this);
            isDisposed = false;
            if (tmpHalfD==default(Vector2)) // for newly created objects, record their smallest demension, and sync with halfD when next time refresh the board 
                                            // it has to be done in this way, because "halfD" cannot be changed during "UpdateBoard()"
            {
                tmpHalfD = new Vector2(Math.Max(e.S.X / 2, 1), Math.Max(e.S.Y / 2, 1)); 
            }
            else
            {
                tmpHalfD = new Vector2(Math.Max(Math.Min(tmpHalfD.X, e.S.X/2), 1), Math.Max(Math.Min(tmpHalfD.Y, e.S.Y/2), 1));
            }
        }
        public virtual void Dispose() //remove "this" from "Arena" and inform other objects may use "this"
        {
            Arena.Remove(this);
            isDisposed = true;
        }
        public static void getItems() //draw all the in-game objects. It is called whenever Game().Draw() was called
        {
            bool isEnd = true;
            foreach (GameEngine x in Arena)
            {
                if (x is Enemy)
                {
                    if ((x as Enemy).hit)
                    {
                        (x as Enemy).hit = false;
                    }
                    else
                    {
                        spriteBatch.Draw(x.Image, new Rectangle((int)x.Position.X, (int)x.Position.Y, (int)x.Size.X, (int)x.Size.Y), Color.White);
                    }
                    isEnd = false;
                }
                else
                {
                    if (!GameEngine.Player.JustRevived || !(x is Bullet)|| (x as Bullet).Flag == Flags.Player)
                    {
                        spriteBatch.Draw(x.Image, new Rectangle((int)x.Position.X, (int)x.Position.Y, (int)x.Size.X, (int)x.Size.Y), Color.White);
                    }
                }
                if (x is Player && !(x as Player).JustRevived && !(x as Player).ImmuDamage) //draw the most updated hitbox
                {
                    Texture2D hitBox = new Texture2D(graphic.GraphicsDevice, 1, 1);
                    hitBox.SetData(new Color[] { Color.Black });
                    //hitBox is little bit smaller than actual for better appearance
                    spriteBatch.Draw(hitBox, new Rectangle((int)(x.Center.X - x.getHalfHitBox.X/2), (int)(x.Center.Y - x.getHalfHitBox.Y/2), (int)x.getHalfHitBox.X, (int)x.getHalfHitBox.Y), Color.Black);
                }
            }

            End = isEnd;
        }
        public static void actItems()//one of the most important method. Called whenever Game().Update() was called. Let all in-game objects perform their actions at the current update tiime
        {
            resetBoard();
            List<GameEngine> tmp = new List<GameEngine> (Arena.Select(x=>x));
            foreach (GameEngine x in tmp)
            {
                if (!GameEngine.Player.JustRevived || !(x is Bullet)|| (x as Bullet).Flag == Flags.Player)
                {
                    x.Action();
                }
                else
                {
                    Arena.Remove(x);
                }
            }
        }

        public static void clearItems()
        {
            boardA = null;
            boardB = null;
            Arena.Clear();
        }

        public Vector2 getHalfHitBox //all other classes (including its derive classes) can only read the "halD" but cannot write/modify
        {
            get
            {
                return halfD;
            }
        }

        public virtual void Die()
        {

        }
        #endregion
        #region Protected

        protected virtual void Action() //Action this in-game object should take in the current update.
                                        //Each single in-game object knows what it should do and does it on its own without our interfere
        {
            if (outOfBoundary()) //if this in-game object COMPLETELY out of game arean, its lifecycle should be terminated and removed
            {
                Die();
            }
        }
        protected virtual void UpdateImage(Texture2D path) //only way to change image
        {
            if (path == null)
            {
                throw new NullReferenceException("Image path is null!");
            }
            img = path;
        }
        protected void UpdatePosition(Vector2 pos) //only way to update its current position/location of the game area
        {
            if (pos == null)
            {
                throw new NullReferenceException("Position is null!");
            }
            Position = pos;
            if (outOfBoundary())
            {
                return;
            }
            UpdateBoard(); //everytime when the position of the in-game object had been updated, it should be trackted by adding it to the "board", if it is Qualified (i.e: bullets, player, enemies&bosses)
        }
        protected void UpdateSize(Vector2 s)
        {
            Size = new Vector2(Math.Max(s.X, 1), Math.Max(s.Y, 1));
        }
        protected virtual void UpdateBoard()
        {
            // "GameEngine" item is not Qualified item. This method will be overrided once for all at "BasicAndOthers.cs"->"GameEngineBehaviors" -> "UpdateBoard()"
        }
        protected bool outOfBoundary()
        {
            return Position.X+Size.X < 0 || Position.Y+Size.Y < 0 || Position.X > graphic.PreferredBackBufferWidth || Position.Y > graphic.PreferredBackBufferHeight;
        }

        protected void incScore()
        {
            if(Score < int.MaxValue)
            {
                Score++;
            }
        }

        #endregion
        private static void resetBoard()
        {
            //sync most-updated minimum demensions
            halfD = tmpHalfD;
            int X = (int)Math.Ceiling(graphic.PreferredBackBufferWidth / (halfD.X * 2))+1; // "+1" because we count from 1 instead of 0
            int Y = (int)Math.Ceiling(graphic.PreferredBackBufferHeight / (halfD.Y * 2))+1;
            boardA = new GameEngine[X, Y]; //refresh both board
            boardB = new Flags[X, Y];
        }
    }
}