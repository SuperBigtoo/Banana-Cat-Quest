using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana_Catto_Quest.GameObjects
{
    internal class MasterLock6 : GameObject
    {
        private List<Texture2D> masterLockTexture;
        private float animationTimer;
        private float timerAnimation;
        private float animationInterval = 0.5f;

        public MasterLock6(List<Texture2D> masterLockTexture) : base(masterLockTexture[0])
        {
            this.masterLockTexture = masterLockTexture;
            color = new Color(77, 77, 77, 100);
        }

        public override void Update(GameTime gameTime)
        {

            timerAnimation += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            // Handle animation
            animationTimer += timerAnimation;

            Animate(masterLockTexture[0]);
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
