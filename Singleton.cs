using System.Collections.Generic;
using Banana_Catto_Quest.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Banana_Catto_Quest
{
    internal class Singleton
    {
        public Vector2 GameScreen = new Vector2(1280, 720);
        public Vector2 velocityUI;

        public Vector2 PlayerPosition;
        public Rectangle PlayerRectangle;
        public int PlayerHP = 100;
        public bool IsPlayerHit;
        public bool IsPlayerDead;
        public int bananaCount = 0;

        public List<Enemy> enemiesList = new List<Enemy>();
        public List<BananaItem> bananaItemList = new List<BananaItem>();
        public List<HeartItem> heartItemList = new List<HeartItem>();

        public KeySection6 keySection6_Obj;
        public MasterLock6 masterLock6_Obj;
        public Door doorSec5;
        public bool IsDoorLock = true;
        public bool keySection6 = false;

        public float SFX_Volume = 0.5f;
        public float BGM_Volume = 0.25f;

        public int loadSection = 1;
        public int playerSectionEntered = 1;

        public bool LoadedSection_2 = false;
        public bool LoadedSection_3 = false;
        public bool LoadedSection_4 = false;
        public bool LoadedSection_5 = false;
        public bool LoadedSection_6 = false;
        public bool LoadedSection_7 = false;
        public bool LoadedSection_8 = false;

        public bool IsGameStart = false;
        public bool IsGamePause = false;
        public bool IsGameWin = false;
        public bool IsGameOver = false;

        public float mainFloor = 485f;

        public MouseState mousePreviose, mouseCurrent;
        public KeyboardState keyboardPreviose, keyboardCurrent;

        private static Singleton instance;

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
