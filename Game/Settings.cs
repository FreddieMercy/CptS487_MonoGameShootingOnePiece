using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TeamWowGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        #region Custom Values
        int playerInitialLives = 5;
        private int PlayerYSpeed = 5;
        private int PlayerXSpeed = 5;
        private Vector2 CloudVelocity = new Vector2(0, 5);
        private Vector2 DefaultPlayerSize = new Vector2(23.8f*4, 32.4f*4);
        private Vector2 DefaultRivalSize = new Vector2(25.6f*4, 36.4f*4);
        private Vector2 DefaultBulletSize = new Vector2(30, 30);
        #endregion
        #region Path of the files in "Content"
        //*********************  Path Field  *********************
        //#images
        private string[] _player_Library = { "img/Humanoid/Luffy", "img/Humanoid/Luffy2", "img/Humanoid/Luffy3"};
        private string[] _e1_Library = { "img/Humanoid/doflamingo", "img/Humanoid/doflamingo2", "img/Humanoid/doflamingo3" };
        private string[] _fb_Library = { "img/Humanoid/final_boss", "img/Humanoid/final_boss2", "img/Humanoid/final_boss3"};
        private string[] _midBoss_Library = { "img/Humanoid/midboss", "img/Humanoid/midboss2", "img/Humanoid/midboss3"};
        private string[] _butterfly_Library = { "img/Humanoid/butterfly", "img/Humanoid/butterfly2", "img/Humanoid/butterfly3" };
        private string[] _lifeStar_Library = { "img/lifeStar", "img/lifeStar2", "img/lifeStar3" };
        private int _gameStyle = 0;

        private string _player = "img/Humanoid/Luffy";
        private string _bg = "img/background/Background";
        private string _c1 = "img/background/C1";
        private string _c2 = "img/background/C2";
        private string _bullet1 = "img/bullets/bullet";
        private string _bomb = "img/bullets/bomb";
        private string _bullet2 = "img/bullets/enemyBullet";
        private string _e1 = "img/Humanoid/doflamingo";
        private string _fb = "img/Humanoid/final_boss";
        private string _sb = "img/bullets/Scattershot_bullet";
        private string _tb = "img/bullets/tracking_Bullet";
        private string _sbw = "img/bullets/Scattershot_wall";
        private string _midBoss = "img/Humanoid/midboss";
        private string _butterfly = "img/Humanoid/butterfly";
        private string _midBossCompanion = "img/Humanoid/midBossCompanion";
        private string _lifeStar = "img/lifeStar";
        
        //#sounds
        private string _BGM = "sounds/BGM";
        private string _Shot = "sounds/shot";
        private float _volume = 0.5f;
        //#Fonts:
        private string _enemyLocater = "fonts/EnemyLocater";
        private string _CheatMode = "fonts/_CheatMode";
        private string _GG = "fonts/GG";
        private string _RemainingLives = "fonts/RemainingLives";
        private string _Score = "fonts/Score";

        //#Xml and Json
        private string _Load = "../../../../Load/";
        private string _Menu_Content = "BH_STG_Menu_Content/";
        private string _kmap_FileName = "Other/kmap.json";
        private string _GenerateWave_FileName = "Gameplay/refresh.xml";
        //****************************************************************
        #endregion

    }
}