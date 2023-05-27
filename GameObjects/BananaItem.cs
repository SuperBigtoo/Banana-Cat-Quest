using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace Banana_Catto_Quest.GameObjects
{
    internal class BananaItem : GameObject
    {
        private List<Texture2D> bananaItemTexture;
        private float animationTimer;
        private float timerAnimation;
        private float animationInterval = 0.5f;
        public SoundEffectInstance collected;

        public BananaItem(List<Texture2D> bananaItemTexture) : base(bananaItemTexture[0])
        {
            this.bananaItemTexture = bananaItemTexture;
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {

            timerAnimation += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            // Handle animation
            animationTimer += timerAnimation;

            Animate(bananaItemTexture[0]);

            if (Rectangle.Intersects(Singleton.Instance.PlayerRectangle))
            {
                collected.Play();
                Singleton.Instance.bananaCount++;
                Singleton.Instance.bananaItemList.Remove(this);
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
