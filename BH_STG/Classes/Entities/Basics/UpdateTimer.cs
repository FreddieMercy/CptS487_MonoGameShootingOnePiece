using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BH_STG
{
    public class UpdateTimer
    {
        private TimeSpan timer = TimeSpan.Zero;
        private TimeSpan interval;
        private bool Running = false;
        public UpdateTimer(TimeSpan i)
        {
            interval = i;
        }

        public bool TimeUp(GameTime gameTime)
        {
            if (Running)
            {
                timer += gameTime.ElapsedGameTime;
                if (timer > interval)
                {
                    timer -= interval;
                    return true;
                }
            }
            return false;
        }

        public void Start()
        {
            Running = true;
        }

        public void Pause()
        {
            Running = false;
        }

        public void Reset()
        {
            timer = TimeSpan.Zero;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
    }
}
