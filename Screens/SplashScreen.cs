using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace Banana_Catto_Quest.Screens
{
    internal class SplashScreen : Screen
    {
        private Color color;

        private Texture2D logo;
        private SpriteFont font;

        private bool show;
        private int alpha;
        private int alphaTime;
        private int index;
        private float timer;
        private float timePerUpdate;

        public SplashScreen()
        {
            show = true;
            timer = 0f;
            timePerUpdate = 0.055f;
            index = 0;
            alpha = 0;
            alphaTime = 0;
            color = new Color(255, 255, 255, alpha);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            font = Content.Load<SpriteFont>("GameFont");
            logo = Content.Load<Texture2D>("logo_wakeup");
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

            if (index == 3) timePerUpdate -= 0.01f;
            if (timer >= timePerUpdate)
            {
                if (show)
                {
                    alphaTime += 5;
                    if (alpha < 255) alpha += 5;
                    if (alphaTime >= 300)
                    {
                        show = false;
                        if (index == 3) ScreenManager.Instance.LoadScreen(ScreenManager.GameScreen.PlayScreen);
                    }
                }
                else
                {
                    alphaTime -= 5;
                    if (alpha > 0) alpha -= 5;
                    if (alphaTime <= 0)
                    {
                        show = true;
                        index++;
                        if (index == 1)
                        {
                            color = Color.CornflowerBlue;
                        }
                    }
                }
                timer -= timePerUpdate;
                color.A = (byte)alpha;
            }

            if (Singleton.Instance.mouseCurrent.LeftButton == ButtonState.Pressed
                    && Singleton.Instance.mousePreviose.LeftButton == ButtonState.Released)
            {
                ScreenManager.Instance.LoadScreen(ScreenManager.GameScreen.PlayScreen);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            String textString1;
            String textString2;
            Vector2 textSize1;
            Vector2 textSize2;
            float totalHeight;
            Vector2 position;
            switch (index)
            {
                case 0:
                    Vector2 logoSize = new Vector2(logo.Width, logo.Height) * 0.5f;
                    position = new Vector2((Singleton.Instance.GameScreen.X - logoSize.X) / 2
                        , (Singleton.Instance.GameScreen.Y - logoSize.Y) / 2);
                    spriteBatch.Draw(logo, position, null, color, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);                 
                    break;

                case 1:
                    textString1 = "This game is a part of";
                    textString2 = "Computer Game Programming : Final assignment";
                    textSize1 = font.MeasureString(textString1);
                    textSize2 = font.MeasureString(textString2);

                    totalHeight = textSize1.Y + textSize2.Y;
                    position = new Vector2((Singleton.Instance.GameScreen.X - textSize1.X) / 2
                        , (Singleton.Instance.GameScreen.Y - totalHeight) / 2);                                    

                    spriteBatch.DrawString(font, textString1, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    position.X += (textSize1.X - font.MeasureString(textString2).X) / 2;
                    position.Y += textSize1.Y;
                    spriteBatch.DrawString(font, textString2, position, new Color(255, 102, 178, alpha), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    break;

                case 2:
                    textString1 = "Presented by";
                    textString2 = "Phakorn Pasawast";
                    textSize1 = font.MeasureString(textString1);
                    textSize2 = font.MeasureString(textString2);

                    totalHeight = textSize1.Y + textSize2.Y;
                    position = new Vector2((Singleton.Instance.GameScreen.X - textSize1.X) / 2
                        , (Singleton.Instance.GameScreen.Y - totalHeight) / 2);
          
                    spriteBatch.DrawString(font, textString1, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    position.X += (textSize1.X - font.MeasureString(textString2).X) / 2;
                    position.Y += textSize1.Y;
                    spriteBatch.DrawString(font, textString2, position, new Color(255, 102, 178, alpha), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    break;
            }
        }
    }
}
