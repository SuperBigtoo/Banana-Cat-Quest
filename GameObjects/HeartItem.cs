using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Banana_Catto_Quest.GameObjects
{
    internal class HeartItem : GameObject
    {
        private List<Texture2D> heartItemTexture;
        private float animationTimer;
        private float timerAnimation;
        private float animationInterval = 0.5f;
        public SoundEffectInstance collected;

        public HeartItem(List<Texture2D> heartItemTexture) : base(heartItemTexture[0])
        {
            this.heartItemTexture = heartItemTexture;
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {

            timerAnimation += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            // Handle animation
            animationTimer += timerAnimation;

            Animate(heartItemTexture[0]);

            if (Rectangle.Intersects(Singleton.Instance.PlayerRectangle))
            {
                collected.Play();
                Singleton.Instance.PlayerHP += 10;
                Singleton.Instance.heartItemList.Remove(this);
            }
        }

        private void Animate(params Texture2D[] textures)
        {
            if (animationTimer > animationInterval)
            {
                // Cycle through the animation textures
                Texture2D currentTexture = textures[(int)(animationTimer / animationInterval) % textures.Length];
                texture = currentTexture;

                animationTimer = 0f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player on the screen at its current position
            spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
        }
    }
}
