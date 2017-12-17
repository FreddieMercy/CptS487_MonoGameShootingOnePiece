using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading;
using System.Xml;

namespace BH_STG
{
    public partial class GenerateWaves : IDisposable //tricking the Milestone2 requirement
    {
        public bool End
        {
            get
            {
                //return Schedule.Count == 0 && lastOne.isDisposed;
                return Schedule.Count == 0;
            }
        }
        private bool started;
        private UpdateTimer Timer;
        private double i;
        private Dictionary<double, Action> Schedule;


        public GenerateWaves()
        {
            started = false;
            Timer = new UpdateTimer(TimeSpan.FromSeconds(.1));
           
            i = 0.0;
            Schedule = new Dictionary<double, Action>();

            XmlTextReader reader = new XmlTextReader(Paths.Load + Paths.GenerateWave_FileName);
            int temp_id = 0;
            int temp_Amount = 0;
            char temp_Type = '\0';
            Texture2D image = Images.Enemy1; //default
            int temp_Size = 0;
            Vector2 rivalSize = DefaultSizes.DefaultRivalSize;
            int temp_Velocity_X = 0;
            int temp_Velocity_Y = 0;
            Vector2 velocity = new Vector2(temp_Velocity_X, temp_Velocity_Y);
            string temp_Behavior = "";
            RegularEnemyBehavior behavior = new RegularEnemyBehavior();
            int temp_Live = 0;
            double temp_startTime = 0.0;
            double temp_endTime = 0.0;
            double temp_timeInterval = 0.0;
            float temp_position_X = 0;
            float temp_position_Y = 0;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "wave":
                            temp_id = int.Parse(reader.GetAttribute(0));
                            break;
                        case "Amount":
                            temp_Amount = int.Parse(reader.ReadElementString());
                            break;
                        case "Type":
                            temp_Type = char.Parse(reader.ReadElementString());

                            if (temp_Type == 'A')
                            {
                                image = Images.Enemy1;
                            }
                            else if (temp_Type == 'B')
                            {
                                image = Images.MidBoss;
                            }
                            else if (temp_Type == 'C')
                            {
                                image = Images.final_boss;
                            }
                            else if (temp_Type == 'D')
                            {
                                image = Images.butterfly;
                            }
                            break;
                        case "Size":
                            temp_Size = int.Parse(reader.ReadElementString());

                            if (temp_Size == 1)
                            {
                                rivalSize = DefaultSizes.DefaultRivalSize;
                            }
                            break;
                        case "Velocity_X":
                            temp_Velocity_X = int.Parse(reader.ReadElementString());
                            break;
                        case "Velocity_Y":
                            temp_Velocity_Y = int.Parse(reader.ReadElementString());

                            velocity = new Vector2(temp_Velocity_X, temp_Velocity_Y);
                            break;
                        case "Behavior":
                            temp_Behavior = reader.ReadElementString();
                            break;
                        case "Live":
                            temp_Live = int.Parse(reader.ReadElementString());
                            break;
                        case "startTime":
                            temp_startTime = double.Parse(reader.ReadElementString());
                            break;
                        case "endTime":
                            temp_endTime = double.Parse(reader.ReadElementString());
                            break;
                        case "timeInterval":
                            temp_timeInterval = double.Parse(reader.ReadElementString());
                            break;
                        case "position_X":
                            temp_position_X = float.Parse(reader.ReadElementString());
                            break;
                        case "position_Y":
                            temp_position_Y = float.Parse(reader.ReadElementString());
                            break;
                        default:
                            break;
                    }
                }

                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    // ********************* generate waves *********************
                    while (temp_startTime < temp_endTime && temp_Amount > 0)
                    {
                        temp_startTime = Math.Round(temp_startTime, 1);
                        Texture2D ti = image;
                        float tx = temp_position_X;
                        float ty = temp_position_Y;
                        int tl = temp_Live;
                        if (temp_Behavior.Equals("toRight"))
                            Schedule[temp_startTime] = (Action)(() => new Enemy(
                                        new Entity(temp_id, ti, DefaultSizes.DefaultRivalSize, velocity, new FromToptoRight(), tl),
                                        new Vector2(tx, ty)
                                ));
                        else if (temp_Behavior.Equals("toLeft"))
                            Schedule[temp_startTime] = (Action)(() => new Enemy(
                                    new Entity(temp_id, ti, DefaultSizes.DefaultRivalSize, velocity, new FromToptoLeft(), tl),
                                    new Vector2(tx, ty)
                            ));
                        else if (temp_Behavior.Equals("toTop"))
                            Schedule[temp_startTime] = (Action)(() => new Enemy(
                                    new Entity(temp_id, ti, DefaultSizes.DefaultRivalSize, velocity, new FromToptoTop(), tl),
                                    new Vector2(tx, ty)
                            ));
                        else if (temp_Behavior.Equals("toDown"))
                            Schedule[temp_startTime] = (Action)(() => new Enemy(
                                    new Entity(temp_id, ti, DefaultSizes.DefaultRivalSize, velocity, new FromToptoDown(), tl),
                                    new Vector2(tx, ty)
                            ));
                        else if (temp_Behavior.Equals("MidBoss"))
                            Schedule[temp_startTime] = (Action)(() => new Enemy(
                                    new Entity(temp_id, ti, DefaultSizes.DefaultRivalSize, velocity, new BossBehavior(temp_Live, TimeSpan.FromSeconds(39), new Queue<Attack>(new List<Attack> { new MidBossAttack() })), tl),
                                    new Vector2(tx, ty)
                            ));
                        else if (temp_Behavior.Equals("FinalBoss"))
                            Schedule[temp_startTime] = (Action)(() => new Enemy(
                                    new Entity(temp_id, ti, DefaultSizes.DefaultRivalSize, velocity, new BossBehavior(temp_Live, TimeSpan.FromMinutes(1), new Queue<Attack>(new List<Attack> { new FinalBossStageOne(), new FinalBossStageTwo(), new FinalBossStageThree(), new FinalBossStageFour() })), tl),
                                    new Vector2(tx, ty)
                            ));
                        else if (temp_Behavior.Equals("Butterfly"))
                            Schedule[temp_startTime] = (Action)(() => new Enemy(
                                    new Entity(temp_id, ti, DefaultSizes.DefaultRivalSize, velocity, new BossBehavior(temp_Live, TimeSpan.FromSeconds(10), new Queue<Attack>(new List<Attack> { new ButterflyAttack() })), tl),
                                    new Vector2(tx, ty)
                            ));


                        temp_startTime = temp_startTime + temp_timeInterval;
                        temp_Amount--;
                    }
                }
            }
        }

        public void Dispose()
        {
            Timer.Stop();
        }

  
        public void Start(GameTime gameTime)
        {
            //called in Game().Update()
            if (!started)
            {
                started = true;
                Timer.Start();
            }
            if (Timer.TimeUp(gameTime))
            {
                i += .1;
            }
            i = Math.Round(i, 1); // i should be 0.59999 or 2.0000002, so should round to .1th
            if (Schedule.ContainsKey(i))
            {
                Schedule[i]();
                Schedule.Remove(i);
            }
        }
    }
}