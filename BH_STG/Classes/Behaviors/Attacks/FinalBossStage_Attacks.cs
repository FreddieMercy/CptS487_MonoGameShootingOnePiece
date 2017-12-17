using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace BH_STG
{
    public abstract class FinalBossStage : Attack
    {
        protected Queue<Attack> stages;
        public FinalBossStage(Queue<Attack> s = null) //shout have the option to have "silent" stage
        {
            stages = s;
        }
        public override void Shoot(GameEngineBehaviors b)
        {
            if (stages != null)
            {
                foreach(Attack i in stages)
                {
                    i.Shoot(b);
                }
            }
        }
    }
    public class FinalBossStageOne : FinalBossStage
    {
        public FinalBossStageOne()
        {
            stages = new Queue<Attack>(new List<Attack> { new ScatterBullets(), new TrackingBullets()});
        }
    }

    public class FinalBossStageTwo : FinalBossStage
    {
        public FinalBossStageTwo()
        {
            stages = new Queue<Attack>(new List<Attack> { new ScatterBullets(), new ScatterBulletsTwo() });
        }
    }

    public class FinalBossStageThree : FinalBossStage
    {
        public FinalBossStageThree()
        {
            stages = new Queue<Attack>(new List<Attack> { new ScatterBullets(), new TrackingBulletsTwo() });
        }
    }

    public class FinalBossStageFour : FinalBossStage
    {
        public FinalBossStageFour()
        {
            stages = new Queue<Attack>(new List<Attack> { new RandomBullets(), new WallBullets() });
        }
    }
}