using Banana_Catto_Quest.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Banana_Catto_Quest.Screens
{
    internal class PlayScreen : Screen
    {
        private const float baseGavityInc = 410f;
        private SpriteFont font;
        private Vector2 healthBarPosition, textHpPosition, bananaCountTexturePosition, bananaCountTextPosition;

        private Player player;
        private List<NPC> npcList;
        
        private List<Texture2D> playerTexture, npcTexture_1, npcTexture_2, npcTexture_3, enemiesTexture
            , incTexture, incPrime, healthBarTexture, BG, doorTexture, keyAndMaster6, itemsTexture_1, itemsTexture_2;
        private SoundEffectInstance crying, walking, jumping, enemyAttack_SFX, enemyDead_SFX, click
            , inc_punch, inc_dead, hapi, item;
        private Texture2D fade_BG, keysbinding, badboi, start, pause;
        private List<Song> BGM;

        private float timer = 0f;
        private float timePerUpdate;

        private bool show, fadeFinish;
        private int alpha, fadeLogo , indexBG8;
        private Color color;
        
        public PlayScreen() 
        { 
            npcList = new List<NPC>();
            BGM = new List<Song>();

            playerTexture = new List<Texture2D>();
            npcTexture_1 = new List<Texture2D>();
            npcTexture_2 = new List<Texture2D>();
            npcTexture_3 = new List<Texture2D>();
            enemiesTexture = new List<Texture2D>();
            incTexture = new List<Texture2D>();
            incPrime = new List<Texture2D>();
            healthBarTexture = new List<Texture2D>();
            BG = new List<Texture2D>();
            doorTexture = new List<Texture2D>();
            keyAndMaster6 = new List<Texture2D>();
            itemsTexture_1 = new List<Texture2D>();
            itemsTexture_2 = new List<Texture2D>();

            show = true;
            fadeFinish = false;
            timer = 0f;
            timePerUpdate = 0.055f;
            alpha = 0;
            fadeLogo = 255;
            color = new Color(100, 149, 237, alpha);
        }

        public override void LoadContent(Camera camera)
        {
            base.LoadContent(camera);
            font = Content.Load<SpriteFont>("GameFont");
            // BG
            BG.Add(Content.Load<Texture2D>("ScreensUsed/BG_game_1"));
            BG.Add(Content.Load<Texture2D>("ScreensUsed/BG_game_2"));
            BG.Add(Content.Load<Texture2D>("ScreensUsed/BG_game_3"));
            BG.Add(Content.Load<Texture2D>("ScreensUsed/BG_game_4"));
            BG.Add(Content.Load<Texture2D>("ScreensUsed/BG_game_5(1)"));
            BG.Add(Content.Load<Texture2D>("ScreensUsed/BG_game_5(2)"));
            BG.Add(Content.Load<Texture2D>("ScreensUsed/BG_game_5(3)"));
            BG.Add(Content.Load<Texture2D>("ScreensUsed/BG_game_5(4)"));

            // Items
            itemsTexture_1.Add(Content.Load<Texture2D>("Objects/items/banana_item"));
            itemsTexture_2.Add(Content.Load<Texture2D>("Objects/items/health_item"));

            // Door
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_1"));
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_2"));
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_3"));
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_4"));
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_5"));
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_6"));
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_7"));
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_8"));
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_9"));
            doorTexture.Add(Content.Load<Texture2D>("ScreensUsed/Entrance/entrance_10"));

            // KeyChocco
            keyAndMaster6.Add(Content.Load<Texture2D>("Objects/chocoBoom"));

            fade_BG = Content.Load<Texture2D>("ScreensUsed/Fade_Black");
            keysbinding = Content.Load<Texture2D>("ScreensUsed/control_keys");
            badboi = Content.Load<Texture2D>("ScreensUsed/I'm a bad boi");
            start = Content.Load<Texture2D>("ScreensUsed/start_game");
            pause = Content.Load<Texture2D>("ScreensUsed/pause_game");
            click = Content.Load<SoundEffect>("Audios/minecraft_click").CreateInstance();
            click.Volume = Singleton.Instance.SFX_Volume;

            // BGM
            BGM.Add(Content.Load<Song>("Audios/[Blue Archive] Theme 64 - Pixel Time - A Retro Adventure by Mitsukiyo"));
            BGM.Add(Content.Load<Song>("Audios/xDeviruchi - Take some rest and eat some food!"));
            BGM.Add(Content.Load<Song>("Audios/xDeviruchi - Decisive Battle"));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Singleton.Instance.BGM_Volume;
            MediaPlayer.Play(BGM[0]);

            // UI health bar
            healthBarTexture.Add(Content.Load<Texture2D>("ScreensUsed/UI/healthbar_100"));
            healthBarTexture.Add(Content.Load<Texture2D>("ScreensUsed/UI/healthbar_80"));
            healthBarTexture.Add(Content.Load<Texture2D>("ScreensUsed/UI/healthbar_60"));
            healthBarTexture.Add(Content.Load<Texture2D>("ScreensUsed/UI/healthbar_40"));
            healthBarTexture.Add(Content.Load<Texture2D>("ScreensUsed/UI/healthbar_20"));
            healthBarTexture.Add(Content.Load<Texture2D>("ScreensUsed/UI/healthbar_0"));

            // player textures
            playerTexture.Add(Content.Load<Texture2D>("Objects/Player/Banana_Idle_1"));     //0
            playerTexture.Add(Content.Load<Texture2D>("Objects/Player/Banana_Idle_2"));     //1
            playerTexture.Add(Content.Load<Texture2D>("Objects/Player/Banana_Walking_1"));  //2
            playerTexture.Add(Content.Load<Texture2D>("Objects/Player/Banana_Walking_2"));  //3
            playerTexture.Add(Content.Load<Texture2D>("Objects/Player/Banana_Jumping"));    //4
            playerTexture.Add(Content.Load<Texture2D>("Objects/Player/Banana_Crying"));     //5
            playerTexture.Add(Content.Load<Texture2D>("Objects/Player/Banana_Hit"));     //6

            // NPC_1 textures
            npcTexture_1.Add(Content.Load<Texture2D>("Objects/NPCs/Apple_catto_Idle_1"));
            npcTexture_1.Add(Content.Load<Texture2D>("Objects/NPCs/Apple_catto_Idle_2"));
            npcTexture_2.Add(Content.Load<Texture2D>("Objects/NPCs/Pepper_catto_Idle_1"));
            npcTexture_2.Add(Content.Load<Texture2D>("Objects/NPCs/Pepper_catto_Idle_2"));
            npcTexture_3.Add(Content.Load<Texture2D>("Objects/NPCs/Stawbery_catto_Idle_1"));
            npcTexture_3.Add(Content.Load<Texture2D>("Objects/NPCs/Stawbery_catto_Idle_2"));

            // enemy textures
            enemiesTexture.Add(Content.Load<Texture2D>("Objects/Enemies/BananaBot/BananaBot_Idle_1"));
            enemiesTexture.Add(Content.Load<Texture2D>("Objects/Enemies/BananaBot/BananaBot_Idle_2"));
            enemiesTexture.Add(Content.Load<Texture2D>("Objects/Enemies/BananaBot/BananaBot_Walking_1"));
            enemiesTexture.Add(Content.Load<Texture2D>("Objects/Enemies/BananaBot/BananaBot_Walking_2"));
            enemiesTexture.Add(Content.Load<Texture2D>("Objects/Enemies/BananaBot/BananaBot_Attacking_1"));
            enemiesTexture.Add(Content.Load<Texture2D>("Objects/Enemies/BananaBot/BananaBot_Attacking_2"));

            // Inc textures
            incTexture.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_Idle_1"));
            incTexture.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_Idle_2"));
            incTexture.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_Walking_1"));
            incTexture.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_Walking_2"));
            incTexture.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_Attacking_1"));
            incTexture.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_Attacking_2"));

            incPrime.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_floating"));
            incPrime.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_floating"));
            incPrime.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_floating"));
            incPrime.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_floating"));
            incPrime.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_Shooting_1"));
            incPrime.Add(Content.Load<Texture2D>("Objects/Enemies/Inc_the_Conqueror/Inc_the_Conqueror_Shooting_2"));

            // SFX
            crying = Content.Load<SoundEffect>("Audios/banana_catto_crying").CreateInstance();
            crying.Volume = Singleton.Instance.SFX_Volume;
            walking = Content.Load<SoundEffect>("Audios/player_walking").CreateInstance();
            walking.Volume = Singleton.Instance.SFX_Volume;
            jumping = Content.Load<SoundEffect>("Audios/player_jumping").CreateInstance();
            jumping.Volume = Singleton.Instance.SFX_Volume;

            enemyAttack_SFX = Content.Load<SoundEffect>("Audios/inc_blasting").CreateInstance();
            enemyAttack_SFX.Volume = Singleton.Instance.SFX_Volume;
            enemyDead_SFX = Content.Load<SoundEffect>("Audios/Zombie Death (Minecraft Sound) - Sound Effect for editing_2").CreateInstance();
            enemyDead_SFX.Volume = Singleton.Instance.SFX_Volume;

            inc_punch = Content.Load<SoundEffect>("Audios/inc_punch").CreateInstance();
            inc_punch.Volume = Singleton.Instance.SFX_Volume;
            inc_dead = Content.Load<SoundEffect>("Audios/inc_died").CreateInstance();
            inc_dead.Volume = Singleton.Instance.SFX_Volume;

            hapi = Content.Load<SoundEffect>("Audios/hapihapihapi").CreateInstance();
            hapi.Volume = Singleton.Instance.SFX_Volume;
            item = Content.Load<SoundEffect>("Audios/orb").CreateInstance();
            item.Volume = Singleton.Instance.SFX_Volume;

            initialGameObject();
        }

        private void initialGameObject()
        {
            player = new Player(playerTexture)
            {
                name = "Player",
                playerDamage = 10,
                position = new Vector2(440, Singleton.Instance.mainFloor),
                crying = this.crying,
                walking = this.walking,
                jumping = this.jumping
            };

            healthBarPosition = new Vector2(player.position.X - 390, 35);
            textHpPosition = new Vector2(player.position.X - 278, 44);
            bananaCountTexturePosition = new Vector2(player.position.X - 375, 100);
            bananaCountTextPosition = new Vector2(player.position.X - 300, 100);

            LoadGameObject();
        }

        private void LoadGameObject()
        {
            switch (Singleton.Instance.loadSection)
            {
                case 1:
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 1,
                        EnemyHP = 100,
                        EnemyDamage = 5,
                        MoveSpeed = 0.025f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(1100, 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });

                    npcList.Add(new NPC(npcTexture_1)
                    {
                        name = "AppleCatto",
                        IsLeft = false,
                        IsRight = true,
                        position = new Vector2(75, Singleton.Instance.mainFloor + 50)
                    });

                    npcList.Add(new NPC(npcTexture_2)
                    {
                        name = "PepperCatto",
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(189, Singleton.Instance.mainFloor)
                    });

                    npcList.Add(new NPC(npcTexture_3)
                    {
                        name = "StawberyCatto",
                        IsLeft = false,
                        IsRight = true,
                        position = new Vector2(280, Singleton.Instance.mainFloor)
                    });

                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(950, Singleton.Instance.mainFloor + 50)
                    });
                    break;

                case 2:
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 2,
                        EnemyHP = 120,
                        EnemyDamage = 5,
                        MoveSpeed = 0.035f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(445 + 1280, 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 2,
                        EnemyHP = 100,
                        EnemyDamage = 10,
                        MoveSpeed = 0.040f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(668 + 1280, 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 2,
                        EnemyHP = 150,
                        EnemyDamage = 5,
                        MoveSpeed = 0.02f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(1067 + 1280, 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(825 + 1280, Singleton.Instance.mainFloor + 50)
                    });
                    break;

                case 3:
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 3,
                        EnemyHP = 300,
                        EnemyDamage = 15,
                        MoveSpeed = 0.035f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(304 + (1280 * 2), 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 3,
                        EnemyHP = 150,
                        EnemyDamage = 5,
                        MoveSpeed = 0.025f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(643 + (1280 * 2), 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 3,
                        EnemyHP = 250,
                        EnemyDamage = 10,
                        MoveSpeed = 0.05f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(854 + (1280 * 2), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemsTexture_2)
                    {
                        collected = item,
                        position = new Vector2(467 + (1280 * 2), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(1056 + (1280 * 2), Singleton.Instance.mainFloor + 50)
                    });
                    break;

                case 4:
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 4,
                        EnemyHP = 200,
                        EnemyDamage = 10,
                        MoveSpeed = 0.02f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(1158 + (1280 * 3), 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 4,
                        EnemyHP = 300,
                        EnemyDamage = 10,
                        MoveSpeed = 0.04f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(352 + (1280 * 3), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 4,
                        EnemyHP = 300,
                        EnemyDamage = 5,
                        MoveSpeed = 0.08f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(866 + (1280 * 3), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemsTexture_2)
                    {   
                        collected = item,
                        position = new Vector2(1158 + (1280 * 3), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {   
                        collected = item,
                        position = new Vector2(731 + (1280 * 3), Singleton.Instance.mainFloor + 50)
                    });
                    break;

                case 5:
                    Singleton.Instance.keySection6_Obj = new KeySection6(keyAndMaster6) 
                    {
                        position = new Vector2(210 + (1280 * 4), 545)
                    };
                    Singleton.Instance.masterLock6_Obj = new MasterLock6(keyAndMaster6)
                    {
                        position = new Vector2(900 + (1280 * 4), 545)
                    };
                    Singleton.Instance.doorSec5 = new Door(doorTexture)
                    {
                        position = new Vector2(1130 + (1280 * 4), 0)
                    };
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 5,
                        EnemyHP = 500,
                        EnemyDamage = 5,
                        MoveSpeed = 0.05f,
                        baseGavity = baseGavityInc,
                        IsLeft = false,
                        IsRight = true,
                        position = new Vector2(761 + (1280 * 4), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 5,
                        EnemyHP = 350,
                        EnemyDamage = 10,
                        MoveSpeed = 0.06f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(900 + (1280 * 4), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 5,
                        EnemyHP = 700,
                        EnemyDamage = 10,
                        MoveSpeed = 0.05f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(1100 + (1280 * 4), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemsTexture_2)
                    {
                        collected = item,
                        position = new Vector2(900 + (1280 * 4), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(194 + (1280 * 4), Singleton.Instance.mainFloor + 50)
                    });
                    break;

                case 6:
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 6,
                        EnemyHP = 500,
                        EnemyDamage = 15,
                        MoveSpeed = 0.035f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(343 + (1280 * 5), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 6,
                        EnemyHP = 300,
                        EnemyDamage = 10,
                        MoveSpeed = 0.05f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(819 + (1280 * 5), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 6,
                        EnemyHP = 250,
                        EnemyDamage = 5,
                        MoveSpeed = 0.085f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(1098 + (1280 * 5), 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(508 + (1280 * 5), Singleton.Instance.mainFloor + 50)
                    });
                    break;

                case 7:
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 7,
                        EnemyHP = 1000,
                        EnemyDamage = 10,
                        MoveSpeed = 0.05f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(362 + (1280 * 6), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 7,
                        EnemyHP = 1000,
                        EnemyDamage = 10,
                        MoveSpeed = 0.05f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(851 + (1280 * 6), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(incTexture)
                    {
                        name = "Inc the Conqueror",
                        EnemySection = 7,
                        EnemyHP = 1000,
                        EnemyDamage = 10,
                        MoveSpeed = 0.05f,
                        baseGavity = baseGavityInc,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(981 + (1280 * 6), baseGavityInc),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemsTexture_2)
                    {
                        collected = item,
                        position = new Vector2(262 + (1280 * 6), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(628 + (1280 * 6), Singleton.Instance.mainFloor + 50)
                    });
                    break;

                case 8:
                    Singleton.Instance.enemiesList.Add(new Enemy(incPrime)
                    {
                        name = "Inc the Conqueror Prime",
                        EnemySection = 8,
                        EnemyHP = 3000,
                        EnemyDamage = 25,
                        MoveSpeed = 0.060f,
                        baseGavity = baseGavityInc - 30,
                        IsLeft = true,
                        IsRight = false,
                        position = new Vector2(860 + (1280 * 7), baseGavityInc - 30),
                        enemyAttack_SFX = inc_punch,
                        enemyDead_SFX = inc_dead,
                        enemyTexture_2 = incTexture,
                        itemDropTexture = itemsTexture_2,
                        itemdrop = item
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(626 + (1280 * 7), Singleton.Instance.mainFloor + 50)
                    });

                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 8,
                        EnemyHP = 350,
                        EnemyDamage = 10,
                        MoveSpeed = 0.05f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        IsActive = true,
                        position = new Vector2(40 + (1280 * 6), 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 8,
                        EnemyHP = 200,
                        EnemyDamage = 5,
                        MoveSpeed = 0.085f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        IsActive = true,
                        position = new Vector2(900 + (1280 * 5), 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });
                    Singleton.Instance.enemiesList.Add(new Enemy(enemiesTexture)
                    {
                        name = "BananaBot",
                        EnemySection = 8,
                        EnemyHP = 100,
                        EnemyDamage = 10,
                        MoveSpeed = 0.06f,
                        baseGavity = 485f,
                        IsLeft = true,
                        IsRight = false,
                        IsActive = true,
                        position = new Vector2(200 + (1280 * 6), 485f),
                        enemyAttack_SFX = this.enemyAttack_SFX,
                        enemyDead_SFX = this.enemyDead_SFX
                    });

                    //spawn
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(950, Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(825 + 1280, Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemsTexture_2)
                    {
                        collected = item,
                        position = new Vector2(467 + (1280 * 2), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(1056 + (1280 * 2), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemsTexture_2)
                    {
                        collected = item,
                        position = new Vector2(1158 + (1280 * 3), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(731 + (1280 * 3), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemsTexture_2)
                    {
                        collected = item,
                        position = new Vector2(900 + (1280 * 4), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(194 + (1280 * 4), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(508 + (1280 * 5), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.heartItemList.Add(new HeartItem(itemsTexture_2)
                    {
                        collected = item,
                        position = new Vector2(262 + (1280 * 6), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(628 + (1280 * 6), Singleton.Instance.mainFloor + 50)
                    });
                    Singleton.Instance.bananaItemList.Add(new BananaItem(itemsTexture_1)
                    {
                        collected = item,
                        position = new Vector2(100 + (1280 * 6), Singleton.Instance.mainFloor + 50)
                    });
                    break;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            if (!Singleton.Instance.IsGameOver && !Singleton.Instance.IsGameWin) 
            {
                if (!Singleton.Instance.IsGameStart)
                {
                    Singleton.Instance.mousePreviose = Singleton.Instance.mouseCurrent;
                    Singleton.Instance.mouseCurrent = Mouse.GetState();

                    player.Update(gameTime);

                    // Update NPCs 
                    if (npcList.Count != 0)
                    {
                        for (int i = 0; i < npcList.Count; i++)
                        {
                            npcList[i].Update(gameTime);
                        }
                    }

                    if (timer >= timePerUpdate)
                    {
                        if (show)
                        {
                            alpha += 5;
                            if (alpha >= 255)
                            {
                                show = false;
                            }
                        }
                        else
                        {
                            alpha -= 5;
                            if (alpha <= 0)
                            {
                                show = true;
                            }
                        }

                        if (!fadeFinish)
                        {
                            fadeLogo -= 5;
                            if (fadeLogo <= 0) fadeFinish = true;
                        }

                        timer -= timePerUpdate;
                        color.A = (byte)alpha;
                    }

                    //Start Game
                    if (fadeFinish)
                    {

                        if (Singleton.Instance.mouseCurrent.LeftButton == ButtonState.Pressed
                            && Singleton.Instance.mousePreviose.LeftButton == ButtonState.Released)
                        {
                            click.Play();
                            Singleton.Instance.IsGameStart = true;
                        }
                    }
                }
                else
                {
                    Singleton.Instance.keyboardPreviose = Singleton.Instance.keyboardCurrent;
                    Singleton.Instance.keyboardCurrent = Keyboard.GetState();

                    if (Singleton.Instance.IsGamePause) // On Pause
                    {
                        if (Singleton.Instance.keyboardCurrent.IsKeyDown(Keys.P)
                        && Singleton.Instance.keyboardPreviose.IsKeyUp(Keys.P))
                        {
                            Singleton.Instance.IsGamePause = false;
                        }

                        OnPause();
                    }
                    else //On Playing
                    {
                        if (Singleton.Instance.keyboardCurrent.IsKeyDown(Keys.P)
                        && Singleton.Instance.keyboardPreviose.IsKeyUp(Keys.P))
                        {
                            Singleton.Instance.IsGamePause = true;
                        }

                        OnPlaying(gameTime);

                        if (Singleton.Instance.loadSection >= 8)
                        {
                            if (timer >= timePerUpdate)
                            {
                                indexBG8++;

                                if (indexBG8 > 3)
                                {
                                    indexBG8 = 0;
                                }

                                timer -= timePerUpdate;
                            }
                        }
                    }
                }
            }
            else if (Singleton.Instance.IsGameOver || Singleton.Instance.IsGameWin)
            {
                Singleton.Instance.mousePreviose = Singleton.Instance.mouseCurrent;
                Singleton.Instance.mouseCurrent = Mouse.GetState();

                if (Singleton.Instance.mouseCurrent.LeftButton == ButtonState.Pressed
                    && Singleton.Instance.mousePreviose.LeftButton == ButtonState.Released)
                {
                    Camera.SetPosition(new Vector2(Singleton.Instance.GameScreen.X / 2, Singleton.Instance.GameScreen.Y / 2));
                    Singleton.Instance.PlayerHP = 100;
                    Singleton.Instance.bananaCount = 0;
                    Singleton.Instance.IsDoorLock = true;
                    Singleton.Instance.keySection6 = false;
                    Singleton.Instance.loadSection = 1;
                    Singleton.Instance.playerSectionEntered = 1;
                    Singleton.Instance.IsGameStart = false;
                    Singleton.Instance.IsGamePause = false;
                    Singleton.Instance.IsGameWin = false;
                    Singleton.Instance.IsGameOver = false;
                    Singleton.Instance.LoadedSection_2 = false;
                    Singleton.Instance.LoadedSection_3 = false;
                    Singleton.Instance.LoadedSection_4 = false;
                    Singleton.Instance.LoadedSection_5 = false;
                    Singleton.Instance.LoadedSection_6 = false;
                    Singleton.Instance.LoadedSection_7 = false;
                    Singleton.Instance.LoadedSection_8 = false;
                    Singleton.Instance.enemiesList.Clear();
                    Singleton.Instance.bananaItemList.Clear();
                    Singleton.Instance.heartItemList.Clear();
                    click.Play();
                    ScreenManager.Instance.LoadScreen(ScreenManager.GameScreen.PlayScreen);
                }
            }

            base.Update(gameTime);
        }

        private void OnPlaying(GameTime gameTime)
        {
            // Update Player
            player.Update(gameTime);

            // Checking Player Entering Section At
            if (player.position.X >= 0 && player.position.X < 1280)
            {
                Singleton.Instance.playerSectionEntered = 1;
            }
            else if (player.position.X >= 1280 && player.position.X < (1280 * 2))
            {
                Singleton.Instance.playerSectionEntered = 2;
            }
            else if (player.position.X >= (1280 * 2) && player.position.X < (1280 * 3))
            {
                Singleton.Instance.playerSectionEntered = 3;
            }
            else if (player.position.X >= (1280 * 3) && player.position.X < (1280 * 4))
            {
                Singleton.Instance.playerSectionEntered = 4;
            }
            else if (player.position.X >= (1280 * 4) && player.position.X < (1280 * 5))
            {
                Singleton.Instance.playerSectionEntered = 5;
            }
            else if (player.position.X >= (1280 * 5) && player.position.X < (1280 * 6))
            {
                Singleton.Instance.playerSectionEntered = 6;
            }
            else if (player.position.X >= (1280 * 6) && player.position.X < (1280 * 7))
            {
                Singleton.Instance.playerSectionEntered = 7;
            }
            else if (player.position.X >= (1280 * 7) && player.position.X < (1280 * 8))
            {
                Singleton.Instance.playerSectionEntered = 8;
            }

            // Update NPCs 
            if (npcList.Count != 0)
            {
                for (int i = 0; i < npcList.Count; i++)
                {
                    npcList[i].Update(gameTime);
                }
            }

            // Update Enemy 
            if (Singleton.Instance.enemiesList.Count != 0)
            {
                for (int i = 0; i < Singleton.Instance.enemiesList.Count; i++)
                {
                    Singleton.Instance.enemiesList[i].Update(gameTime);
                }
            }

            // Update Items
            if (Singleton.Instance.bananaItemList.Count != 0)
            {
                for (int i = 0; i < Singleton.Instance.bananaItemList.Count; i++)
                {
                    Singleton.Instance.bananaItemList[i].Update(gameTime);
                }
            }
            if (Singleton.Instance.heartItemList.Count != 0)
            {
                for (int i = 0; i < Singleton.Instance.heartItemList.Count; i++)
                {
                    Singleton.Instance.heartItemList[i].Update(gameTime);
                }
            }

            // Update camera
            if (player.position.X >= (Singleton.Instance.GameScreen.X * 1) / 2 
                && player.position.X <= 9600)
            {
                Camera.SetPosition(player.position);
            }

            // Update Door, Key and MasterLock Section 5
            if (Singleton.Instance.loadSection >= 5)
            {
                Singleton.Instance.keySection6_Obj.Update(gameTime);
                Singleton.Instance.masterLock6_Obj.Update(gameTime);
                Singleton.Instance.doorSec5.Update(gameTime);
            }

            // Update LoadObject
            if (player.position.X > 960 && Singleton.Instance.loadSection == 1 && !Singleton.Instance.LoadedSection_2)
            {
                Singleton.Instance.loadSection = 2; // loadSection = 2
                LoadGameObject();
                Singleton.Instance.LoadedSection_2 = true;
            }
            else if (player.position.X > 2240 && Singleton.Instance.loadSection == 2 && !Singleton.Instance.LoadedSection_3)
            {
                Singleton.Instance.loadSection = 3; // loadSection = 3
                LoadGameObject();
                Singleton.Instance.LoadedSection_3 = true;
            }
            else if (player.position.X > 3520 && Singleton.Instance.loadSection == 3 && !Singleton.Instance.LoadedSection_4)
            {
                Singleton.Instance.loadSection = 4; // loadSection = 4
                LoadGameObject();
                Singleton.Instance.LoadedSection_4 = true;
            }
            else if (player.position.X > 4800 && Singleton.Instance.loadSection == 4 && !Singleton.Instance.LoadedSection_5)
            {
                Singleton.Instance.loadSection = 5; // loadSection = 5
                LoadGameObject();
                Singleton.Instance.LoadedSection_5 = true;
            }
            else if (player.position.X > 6080 && Singleton.Instance.loadSection == 5 && !Singleton.Instance.LoadedSection_6
                && !Singleton.Instance.IsDoorLock)
            {
                Singleton.Instance.loadSection = 6; // loadSection = 6
                LoadGameObject();
                Singleton.Instance.LoadedSection_6 = true;
            }
            else if (player.position.X > 7360 && Singleton.Instance.loadSection == 6 && !Singleton.Instance.LoadedSection_7)
            {
                Singleton.Instance.loadSection = 7; // loadSection = 7
                LoadGameObject();
                Singleton.Instance.LoadedSection_7 = true;
            }
            else if (player.position.X > 8640 && Singleton.Instance.loadSection == 7 && !Singleton.Instance.LoadedSection_8)
            {
                Singleton.Instance.loadSection = 8; // loadSection = 8
                MediaPlayer.Play(BGM[2]);
                LoadGameObject();
                timer = 0;
                Singleton.Instance.LoadedSection_8 = true;
            }

            // Checking Over
            if (Singleton.Instance.PlayerHP <= 0)
            {
                Singleton.Instance.IsGameOver = true;
                crying.Play();
            }

            // Checking Win
            if (Singleton.Instance.enemiesList.Count == 0 && Singleton.Instance.loadSection == 8)
            {
                Singleton.Instance.IsGameWin = true;
                hapi.Play();
                MediaPlayer.Play(BGM[0]);
            }
        }

        private void OnPause() 
        {
            if (timer >= timePerUpdate)
            {
                if (show)
                {
                    alpha += 5;
                    if (alpha >= 255)
                    {
                        show = false;
                    }
                }
                else
                {
                    alpha -= 5;
                    if (alpha <= 0)
                    {
                        show = true;
                    }
                }

                if (!fadeFinish)
                {
                    fadeLogo -= 5;
                    if (fadeLogo <= 0) fadeFinish = true;
                }

                timer -= timePerUpdate;
                color.A = (byte)alpha;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //BG
            spriteBatch.Draw(BG[0], Vector2.Zero, Color.White);
            spriteBatch.Draw(BG[0], new Vector2(1280, 0), Color.White);
            spriteBatch.Draw(BG[1], new Vector2(1280 * 2, 0), Color.White);
            spriteBatch.Draw(BG[1], new Vector2(1280 * 3, 0), Color.White);
            spriteBatch.Draw(BG[2], new Vector2(1280 * 4, 0), Color.White);
            spriteBatch.Draw(BG[3], new Vector2(1280 * 5, 0), Color.White);
            spriteBatch.Draw(BG[3], new Vector2(1280 * 6, 0), Color.White);

            if (Singleton.Instance.loadSection >= 7)
            {
                switch (indexBG8) 
                {
                    case 0:
                        spriteBatch.Draw(BG[4], new Vector2(1280 * 7, 0), Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(BG[5], new Vector2(1280 * 7, 0), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(BG[6], new Vector2(1280 * 7, 0), Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(BG[7], new Vector2(1280 * 7, 0), Color.White);
                        break;
                }
            }

            // NPCs
            if (npcList.Count != 0)
            {
                for (int i = 0; i < npcList.Count; i++)
                {
                    npcList[i].Draw(spriteBatch);
                }
            }

            // Items
            if (Singleton.Instance.bananaItemList.Count != 0)
            {
                for (int i = 0; i < Singleton.Instance.bananaItemList.Count; i++)
                {
                    Singleton.Instance.bananaItemList[i].Draw(spriteBatch);
                }
            }
            if (Singleton.Instance.heartItemList.Count != 0)
            {
                for (int i = 0; i < Singleton.Instance.heartItemList.Count; i++)
                {
                    Singleton.Instance.heartItemList[i].Draw(spriteBatch);
                }
            }

            // Draw Door, Key and MasterLock Section 5
            if (Singleton.Instance.loadSection >= 5)
            {
                Singleton.Instance.keySection6_Obj.Draw(spriteBatch);
                Singleton.Instance.masterLock6_Obj.Draw(spriteBatch);
                Singleton.Instance.doorSec5.Draw(spriteBatch);
            }

            // Player
            player.Draw(spriteBatch);

            // Enemies
            if (Singleton.Instance.enemiesList.Count != 0)
            {
                for (int i = 0; i < Singleton.Instance.enemiesList.Count; i++)
                {
                    Singleton.Instance.enemiesList[i].Draw(spriteBatch);
                }
            }

            // GameOver
            if (Singleton.Instance.IsGameOver)
            {
                String textString1;
                String textString2;
                Vector2 textSize1;
                Vector2 textSize2;
                float totalHeight;
                Vector2 position;

                textString1 = "Game Over";
                textString2 = "Oh no, no happi catto for ya";
                textSize1 = font.MeasureString(textString1);
                textSize2 = font.MeasureString(textString2);

                totalHeight = textSize1.Y + textSize2.Y;
                position = new Vector2(player.position.X, player.position.Y - totalHeight - 200);

                spriteBatch.DrawString(font, textString1, position, new Color(255, 102, 178), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                position.X += (textSize1.X - font.MeasureString(textString2).X) / 2;
                position.Y += textSize1.Y;
                spriteBatch.DrawString(font, textString2, position, new Color(255, 102, 178), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            // GameWin
            if (Singleton.Instance.IsGameWin)
            {
                String textString1;
                String textString2;
                Vector2 textSize1;
                Vector2 textSize2;
                float totalHeight;
                Vector2 position;

                textString1 = "Game Win!";
                textString2 = "Good boi, ya habe save fruity catto planet from the Conqueror";
                textSize1 = font.MeasureString(textString1);
                textSize2 = font.MeasureString(textString2);

                totalHeight = textSize1.Y + textSize2.Y;
                position = new Vector2(player.position.X, player.position.Y - totalHeight - 200);

                spriteBatch.DrawString(font, textString1, position, new Color(255, 102, 178), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                position.X += (textSize1.X - font.MeasureString(textString2).X) / 2;
                position.Y += textSize1.Y;
                spriteBatch.DrawString(font, textString2, position, new Color(255, 102, 178), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            if (!Singleton.Instance.IsGameStart)
            {
                //fade BG
                spriteBatch.Draw(fade_BG, Vector2.Zero, new Color(fadeLogo, fadeLogo, fadeLogo, fadeLogo));
                //start show
                Vector2 startPosition = new Vector2((Singleton.Instance.GameScreen.X - start.Width) / 2
                        , (Singleton.Instance.GameScreen.Y - start.Height) / 2);
                spriteBatch.Draw(start, startPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(start, startPosition, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                startPosition = new Vector2(50, 30);
                spriteBatch.Draw(keysbinding, startPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                startPosition = new Vector2(945, 343);
                spriteBatch.Draw(badboi, startPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                if (Singleton.Instance.IsGamePause)
                {
                    Vector2 pausePosition = new Vector2(player.position.X, player.position.Y - 200);
                    //start show
                    spriteBatch.Draw(pause, pausePosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(pause, pausePosition, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else
                {
                    if (player.position.X >= Singleton.Instance.GameScreen.X / 2 && player.position.X <= 9600)
                    {
                        healthBarPosition = new Vector2(player.position.X - 590, 35);
                        textHpPosition = new Vector2(player.position.X - 478, 44);
                        bananaCountTexturePosition = new Vector2(player.position.X - 575, 100);
                        bananaCountTextPosition = new Vector2(player.position.X - 500, 100);
                    }
                    //healthBarPosition += Singleton.Instance.velocityUI;
                    int HP = Singleton.Instance.PlayerHP;
                    int bananaCount = Singleton.Instance.bananaCount;

                    if ((HP <= 100) && (HP > 80) || HP > 100)
                    {
                        spriteBatch.Draw(healthBarTexture[0], healthBarPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    else if ((HP <= 80) && (HP > 60))
                    {
                        spriteBatch.Draw(healthBarTexture[1], healthBarPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    else if ((HP <= 60) && (HP > 40))
                    {
                        spriteBatch.Draw(healthBarTexture[2], healthBarPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    else if ((HP <= 40) && (HP > 20))
                    {
                        spriteBatch.Draw(healthBarTexture[3], healthBarPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    else if ((HP <= 20) && (HP > 0))
                    {
                        spriteBatch.Draw(healthBarTexture[4], healthBarPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    else 
                    {
                        spriteBatch.Draw(healthBarTexture[5], healthBarPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }

                    String textString1 = "   "+HP;
                    spriteBatch.DrawString(font, textString1, textHpPosition, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

                    spriteBatch.Draw(itemsTexture_1[0], bananaCountTexturePosition, null, Color.White, 0f, Vector2.Zero, 0.30f, SpriteEffects.None, 0f);
                    
                    textString1 = ""+bananaCount;
                    spriteBatch.DrawString(font, textString1, bananaCountTextPosition, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

                    textString1 = "Player Damage : " + player.playerDamage;
                    spriteBatch.DrawString(font, textString1, new Vector2(bananaCountTexturePosition.X, bananaCountTexturePosition.Y + 50)
                        , Color.White, 0f, Vector2.Zero, 1.75f, SpriteEffects.None, 0f);

                    textString1 = "Knockback Lv. : " + player.knockbackLevel;
                    spriteBatch.DrawString(font, textString1, new Vector2(bananaCountTexturePosition.X, bananaCountTexturePosition.Y + 80)
                        , Color.White, 0f, Vector2.Zero, 1.75f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
