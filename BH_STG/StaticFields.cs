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
    public class Images
    {
        public static Texture2D BG;
        public static Texture2D MyBullet;
        public static Texture2D MyBullet_bomb;
        public static Texture2D butterfly; 
        public static Texture2D EnemyBullet;
        public static Texture2D Enemy1;
        public static Texture2D final_boss;
        public static Texture2D Scattershot_bullet;
        public static Texture2D tracking_Bullet;
        public static Texture2D Scattershot_Bullet_wall;
        public static Texture2D MidBoss;
        public static Texture2D MidBossCompanion;
        public static Texture2D lifeStar;
    }
    public class Sounds
    {
        public static SoundEffect MyBullet;
        public static Song BGM;
    }

    public class DefaultSizes
    {
        public static Vector2 DefaultBulletSize;
        public static Vector2 DefaultRivalSize;
    }

    public class Paths
    {
        public static string Load;
        public static string Menu_Content;
        public static string kmap_FileName;
        public static string GenerateWave_FileName;
    }

    public class Fonts
    {
        public static SpriteFont CheatMode, EnemyLocater, GG, RemainingLives, Score; 
    }

    public class PlayerAttributes
    {
        public static Vector2 DefaultPlayerSize;
        public static Texture2D Image;
        public static int initLives;
        public static Vector2 Velocity;
    }

    public class CloudAttributes
    {
        public static Texture2D Image1;
        public static Texture2D Image2;
        public static Vector2 Velocity;
    }
}
