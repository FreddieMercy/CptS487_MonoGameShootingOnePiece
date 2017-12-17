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
    public class Image : Content_ItemsNeedtoLoadLocalMedias
    {
        public string Path;
        public string Effects;
        public Vector2 Scale;
                
        public float Alpha;
         
        public string Text, FontName;
         
        public Vector2 Position;
         
        public Rectangle SourceRect;
         
        public bool IsActivate;
         
        //Vector2 origin;
         
        RenderTarget2D renderTarget;
         
        SpriteFont font;
         
        Dictionary<string, ImageEffect> effectList;

        public FadeEffect FadeEffect;
         
        public SpriteSheetEffect SpriteSheetEffect;

        [XmlIgnore]
        public Texture2D Texture;
        [XmlIgnore]
        public bool IsFullScreen { get; private set; }

        void SetEffect<T>(ref T effect)
        {
            if (effect == null)
            {
                effect = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            effectList.Add(effect.GetType().ToString().Replace(typeof(MenuComponent).Namespace + ".", ""), (effect as ImageEffect));
        }

        public void ActivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = true;
                var obj = this;
                effectList[effect].LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = false;
                effectList[effect].UnloadContent();
            }
        }

        public void StoreEffects()
        {
            Effects = String.Empty;
            foreach (var effect in effectList)
            {
                if (effect.Value.IsActive)
                {
                    Effects += effect.Key + ":";
                }
            }
            if (Effects != String.Empty)
            {
                Effects.Remove(Effects.Length - 1);
            }
        }
        public void RestoreEffect()
        {
            foreach (var effect in effectList)
            {
                DeactivateEffect(effect.Key);
            }
            string[] split = Effects.Split(':');
            foreach (string s in split)
            {
                ActivateEffect(s);
            }
        }

        public Image()
        {
            Path = Text = Effects = String.Empty;
            FontName = Paths.Menu_Content + "Fonts/Orbitron";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            effectList = new Dictionary<string, ImageEffect>();
            IsFullScreen = false;
        }

        public void LoadContent()
        {
            //content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            //content = ScreenManager.Instance.Content;
            
            if (Path != String.Empty)
            { 
                Texture = Load<Texture2D>(Paths.Menu_Content + Path);
            }
        
            font = Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

            if (Texture != null)
            {
                dimensions.X += Texture.Width;
            }
            dimensions.X += font.MeasureString(Text).X;

            if (Texture != null)
            {
                dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            }
            else
            {
                dimensions.Y = font.MeasureString(Text).Y;
            }
            if (SourceRect == Rectangle.Empty)
            {
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }
            Position = new Vector2((GameEngine.graphic.PreferredBackBufferWidth-dimensions.X)/2, (GameEngine.graphic.PreferredBackBufferHeight-dimensions.Y)/2);
            renderTarget = new RenderTarget2D(GameEngine.graphic.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
            GameEngine.graphic.GraphicsDevice.SetRenderTarget(renderTarget);
            GameEngine.graphic.GraphicsDevice.Clear(Color.Transparent);
            GameEngine.spriteBatch.Begin();
            if (Texture != null)
            {
                GameEngine.spriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            }
            GameEngine.spriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            GameEngine.spriteBatch.End();

            Texture = renderTarget;

            GameEngine.graphic.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref FadeEffect);
            SetEffect<SpriteSheetEffect>(ref SpriteSheetEffect);

            if (Effects != String.Empty)
            {
                String[] split = Effects.Split(':');
                foreach (string item in split)
                {
                    ActivateEffect(item);
                }
            }
        }
        public void UnloadContent()
        {
            Unload();
            foreach (var effect in effectList)
            {
                DeactivateEffect(effect.Key);
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (var effect in effectList)
            {
                if (effect.Value.IsActive)
                {
                    effect.Value.Update(gameTime);
                }
            }
        }
        public void Draw()
        {
            //origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
            GameEngine.spriteBatch.Draw(Texture, Position, SourceRect, Color.White * Alpha, 0.0f, Vector2.Zero, Scale, SpriteEffects.None,0);
        }
        public void ToFullScreen()
        {
            if (!IsFullScreen)
            {
                Position = Vector2.Zero;
                renderTarget = new RenderTarget2D(GameEngine.graphic.GraphicsDevice, GameEngine.graphic.PreferredBackBufferWidth, GameEngine.graphic.PreferredBackBufferHeight);
                Scale = new Vector2(GameEngine.graphic.PreferredBackBufferWidth / SourceRect.Width, GameEngine.graphic.PreferredBackBufferHeight / SourceRect.Height);
                SourceRect = new Rectangle((int)Position.X, (int)Position.Y, GameEngine.graphic.PreferredBackBufferWidth, GameEngine.graphic.PreferredBackBufferHeight);
                IsFullScreen = true;
            }
        }
    }
}
