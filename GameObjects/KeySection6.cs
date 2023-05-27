using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Banana_Catto_Quest.GameObjects
{
    internal class KeySection6 : GameObject
    {
        private List<Texture2D> keyTexture;
        private float animationTimer;
        private float timerAnimation;
        private float animationInterval = 0.5f;

        public KeySection6(List<Texture2D> keyTexture) : base(keyTexture[0])
        {
            this.keyTexture = keyTexture;
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            timerAnimation += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            // Handle animation
            animationTimer += timerAnimation;

            Animate(keyTexture[0]);
            
            if (Rectangle.Intersects(Singleton.Instance.masterLock6_Obj.Rectangle))
            {
                Singleton.Instance.keySection6 = true;
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
            spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
