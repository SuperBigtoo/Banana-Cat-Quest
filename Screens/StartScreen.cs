using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Banana_Catto_Quest.Screens
{
    internal class StartScreen : Screen
    {
        private Texture2D BG, logo, start;
        private Song BGM;
        private SoundEffectInstance click;

        private bool show, fadeFinish;
        private int alpha, fadeLogo;
        private float timer;
        private float timePerUpdate;

        private Color color;

        public StartScreen()
        {
            show = true;
            fadeFinish = false;
            timer = 0f;
            timePerUpdate = 0.055f;
            alpha = 0;
            fadeLogo = 0;
            color = new Color(100, 149, 237, alpha);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            BG = Content.Load<Texture2D>("SplashScreen/SplashBG");
            logo = Content.Load<Texture2D>("SplashScreen/IEB_Logo");
            start = Content.Load<Texture2D>("SplashScreen/start");
            click = Content.Load<SoundEffect>("Audios/minecraft_click").CreateInstance();
            click.Volume = Singleton.Instance.SFX_Volume;

            BGM = Content.Load<Song>("Audios/xDeviruchi - Take some rest and eat some food!");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Singleton.Instance.BGM_Volume;
            MediaPlayer.Play(BGM);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            Singleton.Instance.mousePreviose = Singleton.Instance.mouseCurrent;
            Singleton.Instance.mouseCurrent = Mouse.GetState();

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
                    fadeLogo += 5;
                    if (fadeLogo >= 255) fadeFinish = true;
                }

                timer -= timePerUpdate;
                color.A = (byte)alpha;
            }

            //transition to PlayScreen
            if (fadeFinish)
            {

                if (Singleton.Instance.mouseCurrent.LeftButton == ButtonState.Pressed
                    && Singleton.Instance.mousePreviose.LeftButton == ButtonState.Released)
                {
                    click.Play();
                    ScreenManager.Instance.LoadScreen(ScreenManager.GameScreen.PlayScreen);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BG, Vector2.Zero, Color.White);
            //logo
            spriteBatch.Draw(logo, new Vector2((Singleton.Instance.GameScreen.X - logo.Width) / 2.6f, 100), null
                , new Color(fadeLogo, fadeLogo, fadeLogo, fadeLogo), 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

            //start show
            spriteBatch.Draw(start, new Vector2((Singleton.Instance.GameScreen.X - start.Width) / 2, 450), null
                , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(start, new Vector2((Singleton.Instance.GameScreen.X - start.Width) / 2, 450), null
                , color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
