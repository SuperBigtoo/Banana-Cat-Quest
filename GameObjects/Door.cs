using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace Banana_Catto_Quest.GameObjects
{
    internal class Door : GameObject
    {
        private List<Texture2D> doorTexture;
        private float animationTimer;
        private float timerAnimation;
        private float animationInterval = 1.75f;

        public Door(List<Texture2D> doorTexture) : base(doorTexture[0])
        {
            this.doorTexture = doorTexture;
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {

            timerAnimation += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            // Handle animation
            animationTimer += timerAnimation;

            if (Singleton.Instance.IsDoorLock)
            {
                if (!Singleton.Instance.keySection6)
                {
                    Animate(doorTexture[0]);
                }
                else
                {
                    Animate(doorTexture[0], doorTexture[1], doorTexture[2], doorTexture[3], doorTexture[4],
                        doorTexture[5], doorTexture[6], doorTexture[7], doorTexture[8], doorTexture[9]);
                }
            }
            else
            {
                Animate(doorTexture[9]);
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

                if (texture == doorTexture[9])
                {
                    Singleton.Instance.IsDoorLock = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player on the screen at its current position
            spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
