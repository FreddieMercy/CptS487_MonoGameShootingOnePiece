using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BH_STG
{
    public class SpriteSheetEffect : ImageEffect
    {
        public int FrameCounter;
        public int SwithchFrame;
        public Vector2 CurrentFrame;
        public Vector2 AmountOfErames;

        public int FrameWidth
        {
            get
            {
                if (image.Texture != null)
                {
                    return image.Texture.Width / (int)AmountOfErames.X;
                }
                return 0;
            }
        }

        public int FrameHeight
        {
            get
            {
                if (image.Texture != null)
                {
                    return image.Texture.Height / (int)AmountOfErames.Y;
                }
                return 0;
            }
        }
 
        public SpriteSheetEffect()
        {
            AmountOfErames = new Vector2(3, 4);
            CurrentFrame = new Vector2(1, 0);
            SwithchFrame = 100;
            FrameCounter = 0;
        }

        public override void LoadContent(ref Image Image)
        {
            base.LoadContent(ref Image);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (image.IsActivate)
            {
                FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (FrameCounter >= SwithchFrame)
                {
                    FrameCounter = 0;
                    CurrentFrame.X++;

                    if (CurrentFrame.X * FrameWidth >= image.Texture.Width)
                    {
                        CurrentFrame.X = 0;
                    }
                }
            }
            else
            {
                CurrentFrame.X = 1;
            }
            image.SourceRect = new Rectangle((int)CurrentFrame.X * FrameWidth, (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}
